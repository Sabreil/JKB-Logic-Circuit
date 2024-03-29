﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LogicCircuit {
	[SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
	public partial class Editor : EditorDiagram, INotifyPropertyChanged {

		public event PropertyChangedEventHandler PropertyChanged;

		public string File { get; private set; }
		private int savedVersion;
		private int autoSavedVersion;

		public CircuitDescriptorList CircuitDescriptorList { get; private set; }
		private Switcher switcher;

		public bool HasChanges { get { return this.savedVersion != this.CircuitProject.Version; } }
		public string Caption { get { return Properties.Resources.MainFrameCaption(this.File); } }

		// use process specific id in order to prevent dragging and dropping between processes.
		private const double DragStartProximity = 3;
		private Point dragStart;
		private FrameworkElement dragSource;

		private CircuitRunner circuitRunner;
		public CircuitRunner CircuitRunner {
			get { return this.circuitRunner; }
			private set {
				if(this.circuitRunner != value) {
					this.circuitRunner = value;
					this.NotifyPropertyChanged("CircuitRunner");
				}
			}
		}

		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		public bool Power {
			get { return this.CircuitRunner != null; }
			set {
				try {
					if(this.Power != value) {
						if(value) {
							this.CancelMove();
							this.ClearSelection();
							this.CircuitRunner = new CircuitRunner(this);
							this.CircuitRunner.Start();
						} else {
							this.CircuitRunner.Stop();
							this.CircuitRunner = null;
						}
					}
				} catch(Exception exception) {
					this.Mainframe.ReportException(exception);
				}
				this.NotifyPropertyChanged("Power");
			}
		}

		public override bool InEditMode { get { return !this.Power; } }

		public LambdaUICommand CommandUndo => new LambdaUICommand(Properties.Resources.CommandEditUndo, o => this.CanUndo(), o => this.Undo(), new KeyGesture(Key.Z, ModifierKeys.Control));
		public LambdaUICommand CommandRedo => new LambdaUICommand(Properties.Resources.CommandEditRedo, o => this.CanRedo(), o => this.Redo(), new KeyGesture(Key.Y, ModifierKeys.Control));

		public LambdaUICommand CommandCut => new LambdaUICommand(Properties.Resources.CommandEditCut, o => this.InEditMode && 0 < this.SelectionCount, o => this.Cut(), new KeyGesture(Key.X, ModifierKeys.Control));
		public LambdaUICommand CommandCopy => new LambdaUICommand(Properties.Resources.CommandEditCopy, o => this.InEditMode && 0 < this.SelectionCount, o => this.Copy(), new KeyGesture(Key.C, ModifierKeys.Control));
		public LambdaUICommand CommandPaste => new LambdaUICommand(Properties.Resources.CommandEditPaste, o => this.InEditMode && Editor.CanPaste(), o => this.Paste(), new KeyGesture(Key.V, ModifierKeys.Control));
		public LambdaUICommand CommandDelete => new LambdaUICommand(Properties.Resources.CommandEditDelete, o => this.InEditMode && 0 < this.SelectionCount, o => this.Delete(), new KeyGesture(Key.Delete));

		public LambdaUICommand CommandSelectAll => new LambdaUICommand(Properties.Resources.CommandEditSelectAll, o => this.InEditMode, o => this.SelectAll(), new KeyGesture(Key.A, ModifierKeys.Control));
		public LambdaUICommand CommandSelectAllWires => new LambdaUICommand(Properties.Resources.CommandEditSelectAllWires, o => this.InEditMode, o => this.SelectAllWires());
		public LambdaUICommand CommandSelectFreeWires => new LambdaUICommand(Properties.Resources.CommandEditSelectFreeWires, o => this.InEditMode, o => {
			int count = this.SelectFreeWires();
			if(0 < count) {
				this.Mainframe.Status = Properties.Resources.MessageFreeWireCount(count);
			}
		});
		public LambdaUICommand CommandSelectFloatingSymbols => new LambdaUICommand(Properties.Resources.CommandEditSelectFloatingSymbols, o => this.InEditMode,
		o => {
			int count = this.SelectFloatingSymbols();
			if(0 < count) {
				this.Mainframe.Status = Properties.Resources.MessageFloatingSymbolCount(count);
			}
		});
		public LambdaUICommand CommandSelectAllButWires => new LambdaUICommand(Properties.Resources.CommandEditSelectAllButWires, o => this.InEditMode, o => this.SelectAllButWires());
		public LambdaUICommand CommandUnselectAllWires => new LambdaUICommand(Properties.Resources.CommandEditUnselectAllWires, o => this.InEditMode && 0 < this.SelectionCount, o => this.UnselectAllWires());
		public LambdaUICommand CommandUnselectAllButWires => new LambdaUICommand(Properties.Resources.CommandEditUnselectAllButWires, o => this.InEditMode && 0 < this.SelectionCount, o => this.UnselectAllButWires());
		public LambdaUICommand CommandSelectAllProbes => new LambdaUICommand(Properties.Resources.CommandEditSelectAllProbes, o => this.InEditMode, o => this.SelectAllProbes(false));
		public LambdaUICommand CommandSelectAllProbesWithWire => new LambdaUICommand(Properties.Resources.CommandEditSelectAllProbesWithWire, o => this.InEditMode, o => this.SelectAllProbes(true));
		public LambdaUICommand CommandRotateLeft => new LambdaUICommand(Properties.Resources.CommandEditRotateLeft, o => this.InEditMode && this.SelectionCount == 1, o => {
			IRotatable symbol = this.SelectedSymbols.FirstOrDefault() as IRotatable;
			if(symbol != null) {
				this.RotateLeft(symbol, (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.None);
			}
		}, new KeyGesture(Key.L, ModifierKeys.Control));
		public LambdaUICommand CommandRotateRight => new LambdaUICommand(Properties.Resources.CommandEditRotateRight, o => this.InEditMode && this.SelectionCount == 1, o => {
			IRotatable symbol = this.SelectedSymbols.FirstOrDefault() as IRotatable;
			if(symbol != null) {
				this.RotateRight(symbol, (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.None);
			}
		}, new KeyGesture(Key.R, ModifierKeys.Control));

		public LambdaUICommand CommandCircuitProject => new LambdaUICommand(Properties.Resources.CommandCircuitProject, o => this.InEditMode, o => this.Edit(this.CircuitProject.ProjectSet.Project));
		public LambdaUICommand CommandCircuitCurrent => new LambdaUICommand(Properties.Resources.CommandCircuitCurrent, o => this.InEditMode, o => this.Edit(this.Project.LogicalCircuit));
		public LambdaUICommand CommandCircuitNew => new LambdaUICommand(Properties.Resources.CommandCircuitNew, o => this.InEditMode,
			o => this.CircuitProject.InTransaction(() => this.OpenLogicalCircuit(this.CircuitProject.LogicalCircuitSet.Create()))
		);
		public LambdaUICommand CommandCircuitDelete => new LambdaUICommand(Properties.Resources.CommandCircuitDelete, o => this.InEditMode && 1 < this.CircuitProject.LogicalCircuitSet.Count(),
			o => this.DeleteLogicalCircuit()
		);
		public LambdaUICommand CommandCircuitUsage => new LambdaUICommand(Properties.Resources.CommandCircuitUsage, o => this.InEditMode, o => this.LogicalCircuitUsage(this.Project.LogicalCircuit));
		public LambdaUICommand CommandFind => new LambdaUICommand(Properties.Resources.CommandEditFind, o => this.InEditMode, o => this.Find(), new KeyGesture(Key.F, ModifierKeys.Control));
		public LambdaUICommand CommandTruthTable => new LambdaUICommand(Properties.Resources.CommandTruthTable, o => {
			if(CircuitTestSocket.IsTestable(this.Project.LogicalCircuit)) {
				this.Mainframe.ShowDialog(new DialogTruthTable(this.Project.LogicalCircuit));
			} else {
				this.Mainframe.InformationMessage(Properties.Resources.MessageInputOutputPinsMissing);
			}
		}, new KeyGesture(Key.T, ModifierKeys.Control));
		public LambdaUICommand CommandPower => new LambdaUICommand(Properties.Resources.CommandCircuitPower, o => this.Power = !this.Power, new KeyGesture(Key.W, ModifierKeys.Control));

		public LambdaUICommand CommandReport => new LambdaUICommand(Properties.Resources.CommandToolsReport, o => {
			LogicalCircuit root = this.Project.LogicalCircuit;
			if(this.CircuitRunner != null) {
				CircuitMap map = this.CircuitRunner.VisibleMap;
				if(map != null && !this.InEditMode) {
					map = map.Root;
					root = map.Circuit;
				}
			}
			this.Mainframe.ShowDialog(new DialogReport(root));
		});
		public LambdaUICommand CommandOscilloscope => new LambdaUICommand(Properties.Resources.CommandToolsOscilloscope,
			o => this.Power && this.CircuitRunner.HasProbes && this.CircuitRunner.DialogOscilloscope == null,
			o => this.CircuitRunner.ShowOscilloscope()
		);

		private static CircuitProject Create(Mainframe mainframe, string file) {
			bool useAutoSaveFile = false;
			string autoSaveFile = Mainframe.AutoSaveFile(file);
			if(Mainframe.IsFileExists(autoSaveFile)) {
				App.Dispatch(() => {
					MessageBoxResult result = DialogMessage.Show(
						mainframe,
						Properties.Resources.TitleApplication,
						Properties.Resources.MessageLoadAutoSavedFile(file),
						null,
						MessageBoxImage.Question,
						MessageBoxButton.YesNo
					);
					if(result == MessageBoxResult.Yes) {
						useAutoSaveFile = true;
					}
				});
				if(!useAutoSaveFile) {
					Mainframe.DeleteFile(autoSaveFile);
				}
			}
			if(!useAutoSaveFile) {
				autoSaveFile = file;
			}
			CircuitProject project = CircuitProject.Create(autoSaveFile);
			if(useAutoSaveFile) {
				project.InOmitTransaction(() => {});
			}
			return project;
		}

		public Editor(Mainframe mainframe, string file) : base(mainframe, Editor.Create(mainframe, file)) {
			this.File = file;
			// Assume loading taken only one transaction. If auto saved file is loaded a new empty transaction is created, so set this to 1 to mark store dirty.
			this.savedVersion = 1;
			this.CircuitDescriptorList = new CircuitDescriptorList(this.CircuitProject);
			this.switcher = new Switcher(this);
		}

		public void Save(string file) {
			this.Mainframe.ResetAutoSaveTimer();
			string oldFile = this.File;
			if(System.IO.File.Exists(file)) {
				string temp = Mainframe.TempFile(file);
				string backup = null;
				if(Settings.User.CreateBackupFileOnSave) {
					backup = Mainframe.BackupFile(file);
				}
				this.CircuitProject.Save(temp);
				System.IO.File.Replace(temp, file, backup);
			} else {
				this.CircuitProject.Save(file);
			}
			this.File = file;
			this.savedVersion = this.CircuitProject.Version;
			Mainframe.DeleteAutoSaveFile(oldFile);

			this.NotifyPropertyChanged("Caption");
		}

		public void AutoSave() {
			try {
				if(!string.IsNullOrEmpty(this.File) && this.HasChanges && this.autoSavedVersion != this.CircuitProject.Version) {
					string file = Mainframe.AutoSaveFile(this.File);
					if(!string.IsNullOrEmpty(file)) {
						Mainframe.DeleteFile(file);
						this.CircuitProject.SaveSnapshot(file);
						Mainframe.Hide(file);
					}
					this.autoSavedVersion = this.CircuitProject.Version;
				}
			} catch(Exception exception) {
				Tracer.Report("Editor.AutoSave", exception);
			}
		}

		protected override void OnProjectPropertyChanged(string propertyName) {
			switch(propertyName) {
			case "Zoom":
			case "Frequency":
			case "IsMaximumSpeed":
				this.NotifyPropertyChanged(propertyName);
				break;
			}
			base.OnProjectPropertyChanged(propertyName);
		}

		private void NotifyPropertyChanged(string propertyName) {
			this.Mainframe.NotifyPropertyChanged(this.PropertyChanged, this, propertyName);
		}

		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		public double Zoom {
			get { return this.Project.Zoom; }
			set {
				if(this.Zoom != value) {
					try {
						this.CircuitProject.InTransaction(() => this.Project.Zoom = value);
					} catch(Exception exception) {
						this.Mainframe.ReportException(exception);
					}
				}
			}
		}

		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		public int Frequency {
			get { return this.Project.Frequency; }
			set {
				if(this.Frequency != value) {
					try {
						this.CircuitProject.InTransaction(() => this.Project.Frequency = value);
					} catch(Exception exception) {
						this.Mainframe.ReportException(exception);
					}
				}
			}
		}

		private double actualFrequency = 0;
		public double ActualFrequency {
			get { return this.actualFrequency; }
			set {
				if(this.actualFrequency != value) {
					this.actualFrequency = value;
					this.NotifyPropertyChanged("ActualFrequency");
				}
			}
		}

		[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		public bool IsMaximumSpeed {
			get { return this.Project.IsMaximumSpeed; }
			set {
				if(this.IsMaximumSpeed != value) {
					try {
						this.CircuitProject.InTransaction(() => this.Project.IsMaximumSpeed = value);
					} catch(Exception exception) {
						this.Mainframe.ReportException(exception);
					}
				}
			}
		}

		public void OpenLogicalCircuit(CircuitMap map) {
			Tracer.Assert(this.Power);
			this.OpenLogicalCircuit(map.Circuit);
			this.CircuitRunner.VisibleMap = map;
			map.TurnOn();
			map.Redraw(true);
			this.Mainframe.Status = map.Path();
		}

		protected override void UpdateGlyph(LogicalCircuit logicalCircuit) {
			this.CircuitDescriptorList.UpdateGlyph(logicalCircuit);
		}

		public void FullRefresh() {
			foreach(CircuitSymbol symbol in this.CircuitProject.CircuitSymbolSet) {
				symbol.Reset();
			}
			foreach(Wire wire in this.CircuitProject.WireSet) {
				wire.Reset();
			}
			this.Refresh();
			this.CircuitDescriptorList.Refresh();
		}

		//--- Edit Operation

		public void Import(string file) {
			this.CancelMove();
			this.ClearSelection();
			DialogImport dialog = new DialogImport(file, this.CircuitProject);
			bool? result = this.Mainframe.ShowDialog(dialog);
			if(result.HasValue && result.Value) {
				LogicalCircuit target = this.Project.LogicalCircuit;
				this.CircuitProject.InTransaction(() => {
					foreach(LogicalCircuit circuit in dialog.ImportList) {
						circuit.CopyTo(target);
					}
				});
			}
		}

		public RenderTargetBitmap ExportImage() {
			Rect rect = new Rect();
			bool isEmpty = true;
			LogicalCircuit logicalCircuit = this.Project.LogicalCircuit;
			foreach(Wire wire in logicalCircuit.Wires()) {
				Rect wireRect = new Rect(Symbol.ScreenPoint(wire.Point1), Symbol.ScreenPoint(wire.Point2));
				if(isEmpty) {
					rect = wireRect;
					isEmpty = false;
				} else {
					rect.Union(wireRect);
				}
			}
			foreach(CircuitSymbol symbol in logicalCircuit.CircuitSymbols()) {
				Rect symbolRect = new Rect(Symbol.ScreenPoint(symbol.Point), new Size(symbol.Glyph.Width, symbol.Glyph.Height));
				if(symbol.Rotation != Rotation.Up) {
					symbolRect = Symbol.Transform(symbolRect, Symbol.RotationTransform(symbol.Rotation, symbol.X, symbol.Y, symbol.Circuit.SymbolWidth, symbol.Circuit.SymbolHeight));
				}
				if(isEmpty) {
					rect = symbolRect;
					isEmpty = false;
				} else {
					rect.Union(symbolRect);
				}
			}
			foreach(TextNote symbol in logicalCircuit.TextNotes()) {
				Rect symbolRect = new Rect(Symbol.ScreenPoint(symbol.Point), new Size(symbol.Glyph.Width, symbol.Glyph.Height));
				if(symbol.Rotation != Rotation.Up) {
					symbolRect = Symbol.Transform(symbolRect, Symbol.RotationTransform(symbol.Rotation, symbol.X, symbol.Y, symbol.Width, symbol.Height));
				}
				if(isEmpty) {
					rect = symbolRect;
					isEmpty = false;
				} else {
					rect.Union(symbolRect);
				}
			}
			if(!isEmpty) {
				this.ClearSelection();
				Canvas diagram = this.Diagram;
				Brush oldBackground = diagram.Background;
				Transform oldRenderTransform = diagram.RenderTransform;
				Transform oldLayoutTransform = diagram.LayoutTransform;
				double horizontalOffset = 0;
				double verticalOffset = 0;
				ScrollViewer scrollViewer = diagram.Parent as ScrollViewer;
				try {
					if(scrollViewer != null) {
						horizontalOffset = scrollViewer.HorizontalOffset;
						verticalOffset = scrollViewer.VerticalOffset;
						scrollViewer.ScrollToHorizontalOffset(0);
						scrollViewer.ScrollToVerticalOffset(0);
						scrollViewer.UpdateLayout();
					}
					diagram.Background = Brushes.White;
					rect.Inflate(Symbol.GridSize, Symbol.GridSize);
					rect.Intersect(new Rect(0, 0, Symbol.LogicalCircuitWidth, Symbol.LogicalCircuitHeight));
					diagram.RenderTransform = new TranslateTransform(-rect.X, -rect.Y);
					diagram.LayoutTransform = Transform.Identity;
					diagram.UpdateLayout();
					RenderTargetBitmap bitmap = new RenderTargetBitmap(
						(int)Math.Round(rect.Width), (int)Math.Round(rect.Height), 96, 96, PixelFormats.Pbgra32
					);
					bitmap.Render(diagram);
					return bitmap;
				} finally {
					diagram.Background = oldBackground;
					diagram.RenderTransform = oldRenderTransform;
					diagram.LayoutTransform = oldLayoutTransform;
					diagram.UpdateLayout();
					if(scrollViewer != null) {
						scrollViewer.ScrollToHorizontalOffset(horizontalOffset);
						scrollViewer.ScrollToVerticalOffset(verticalOffset);
						scrollViewer.UpdateLayout();
					}
				}
			}
			return null;
		}

		public void Undo() {
			if(this.CanUndo()) {
				this.CancelMove();
				this.ClearSelection();
				this.CircuitProject.Undo();
			}
		}

		public void Redo() {
			if(this.CanRedo()) {
				this.CancelMove();
				this.ClearSelection();
				this.CircuitProject.Redo();
			}
		}

		public bool CanUndo() {
			return this.InEditMode && this.CircuitProject.CanUndo;
		}

		public bool CanRedo() {
			return this.InEditMode && this.CircuitProject.CanRedo;
		}

		public void OpenLogicalCircuit(LogicalCircuit logicalCircuit) {
			this.CancelMove();
			this.ClearSelection();
			if(logicalCircuit != this.Project.LogicalCircuit) {
				bool success = false;
				bool started = false;
				try {
					if(!this.CircuitProject.IsEditor) {
						started = this.CircuitProject.StartTransaction();
					}
					if(this.CircuitProject.IsEditor) {
						this.Project.LogicalCircuit = logicalCircuit;
						if(started) {
							this.CircuitProject.PrepareCommit();
						}
						success = true;
					}
				} finally {
					if(started) {
						if(success) {
							this.CircuitProject.Commit();
						} else {
							this.CircuitProject.Rollback();
						}
					}
				}
			}
		}

		public void DeleteLogicalCircuit() {
			if(1 < this.CircuitProject.LogicalCircuitSet.Count()) {
				this.CancelMove();
				this.ClearSelection();
				LogicalCircuit current = this.Project.LogicalCircuit;
				LogicalCircuit other = this.switcher.SuggestNext();
				Tracer.Assert(other != null && other != current);
				this.CircuitProject.InTransaction(() => {
					this.Project.LogicalCircuit = other;
					current.Delete();
				});
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
		public void Copy() {
			this.CancelMove();
			if(0 < this.SelectionCount) {
				Clipboard.SetDataObject(this.CircuitProject.WriteToString(this.SelectedSymbols), false);
			}
		}

		public static bool CanPaste() {
			return CircuitProject.CanPaste(Clipboard.GetText());
		}

		public void Paste() {
			this.CancelMove();
			this.ClearSelection();
			IEnumerable<Symbol> result = CircuitProject.Paste(Clipboard.GetText());
			Tracer.Assert(result.All(symbol => symbol.LogicalCircuit == this.Project.LogicalCircuit));
			this.EnsureVisible(result);
			this.Select(result);
		}

		private void EnsureVisible(IEnumerable<Symbol> symbols) {
			ScrollViewer scrollViewer = this.Diagram.Parent as ScrollViewer;
			if(scrollViewer != null) {
				double zoom = this.Zoom;
				Rect rect = symbols.Select(s => s.Bounds()).Aggregate((r1, r2) => Rect.Union(r1, r2));
				rect = new Rect(rect.X * zoom, rect.Y * zoom, rect.Width * zoom, rect.Height * zoom);
				Rect view = new Rect(scrollViewer.HorizontalOffset, scrollViewer.VerticalOffset, scrollViewer.ViewportWidth, scrollViewer.ViewportHeight);
				if(!view.IntersectsWith(rect)) {
					double x, y;
					if(rect.X < view.X) {
						x = Math.Min(rect.X + rect.Width / 2 - view.X, rect.Right - view.X - view.Width / 2) + scrollViewer.HorizontalOffset;
					} else {
						x = Math.Min(rect.X - view.X - view.Width / 2, rect.X + rect.Width / 2 - view.Right) + scrollViewer.HorizontalOffset;
					}
					if(rect.Y < view.Y) {
						y = Math.Min(rect.Y + rect.Height / 2 - view.Y, rect.Bottom - view.Y - view.Height / 2) + scrollViewer.VerticalOffset;
					} else {
						y = Math.Min(rect.Y - view.Y - view.Height / 2, rect.Y + rect.Height / 2 - view.Bottom) + scrollViewer.VerticalOffset;
					}
					scrollViewer.ScrollToHorizontalOffset(x);
					scrollViewer.ScrollToVerticalOffset(y);
					this.Project.LogicalCircuit.ScrollOffset = new Point(x, y);
				}
			}
		}

		public void Delete() {
			this.CancelMove();
			if(0 < this.SelectionCount) {
				IEnumerable<Symbol> selection = this.Selection();
				this.ClearSelection();
				this.CircuitProject.InTransaction(() => {
					foreach(Symbol symbol in selection) {
						symbol.DeleteSymbol();
					}
				});
			}
		}

		public void Cut() {
			this.CancelMove();
			if(0 < this.SelectionCount) {
				this.Copy();
				this.Delete();
			}
		}

		public void Edit(Project project) {
			Tracer.Assert(project == this.Project);
			this.Mainframe.ShowDialog(new DialogProject(project));
		}

		public override void Edit(LogicalCircuit logicalCircuit) {
			this.Mainframe.ShowDialog(new DialogCircuit(logicalCircuit));
		}

		public void LogicalCircuitUsage(LogicalCircuit logicalCircuit) {
			this.Mainframe.ShowDialog(new DialogUsage(logicalCircuit));
		}

		private void Edit(CircuitProbe probe) {
			this.Mainframe.ShowDialog(new DialogProbe(probe));
		}

		private void Edit(CircuitButton button) {
			this.Mainframe.ShowDialog(new DialogButton(button));
		}

		private void Edit(Constant constant) {
			this.Mainframe.ShowDialog(new DialogConstant(constant));
		}

		private void Edit(Sensor sensor) {
			this.Mainframe.ShowDialog(new DialogSensor(sensor));
		}

		private void Edit(Memory memory) {
			this.Mainframe.ShowDialog(memory.Writable ? (Window)new DialogRam(memory) : (Window)new DialogRom(memory));
		}

		private void Edit(Pin pin) {
			this.Mainframe.ShowDialog(new DialogPin(pin));
		}

		private void Edit(LedMatrix ledMatrix) {
			this.Mainframe.ShowDialog(new DialogLedMatrix(ledMatrix));
		}

		private void Edit(Sound sound) {
			this.Mainframe.ShowDialog(new DialogSound(sound));
		}

		private void Edit(GraphicsArray graphicsArray) {
			this.Mainframe.ShowDialog(new DialogGraphicsArray(graphicsArray));
		}

		private void Edit(TextNote textNote) {
			DialogText dialog = new DialogText(textNote.Note);
			bool? result = this.Mainframe.ShowDialog(dialog);
			if(result.HasValue && result.Value && !StringComparer.Ordinal.Equals(textNote.Note, dialog.Document)) {
				if(TextNote.IsValidText(dialog.Document)) {
					this.CircuitProject.InTransaction(() => { textNote.Note = dialog.Document; });
				} else {
					this.CircuitProject.InTransaction(() => textNote.Delete());
				}
			}
		}

		[SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
		protected override void Edit(Symbol symbol) {
			CircuitSymbol circuitSymbol = symbol as CircuitSymbol;
			if(circuitSymbol != null) {
				if(this.InEditMode) {
					LogicalCircuit lc = circuitSymbol.Circuit as LogicalCircuit;
					if(lc != null) {
						this.OpenLogicalCircuit(lc);
						return;
					}
					CircuitProbe cp = circuitSymbol.Circuit as CircuitProbe;
					if(cp != null) {
						this.Edit(cp);
						return;
					}
					CircuitButton cb = circuitSymbol.Circuit as CircuitButton;
					if(cb != null) {
						this.Edit(cb);
						return;
					}
					Constant ct = circuitSymbol.Circuit as Constant;
					if(ct != null) {
						this.Edit(ct);
						return;
					}
					Sensor sr = circuitSymbol.Circuit as Sensor;
					if(sr != null) {
						this.Edit(sr);
						return;
					}
					Memory m = circuitSymbol.Circuit as Memory;
					if(m != null) {
						this.Edit(m);
						return;
					}
					Pin pin = circuitSymbol.Circuit as Pin;
					if(pin != null) {
						this.Edit(pin);
						return;
					}
					LedMatrix ledMatrix = circuitSymbol.Circuit as LedMatrix;
					if(ledMatrix != null) {
						this.Edit(ledMatrix);
						return;
					}
					Sound sound = circuitSymbol.Circuit as Sound;
					if(sound != null) {
						this.Edit(sound);
						return;
					}
					GraphicsArray graphicsArray = circuitSymbol.Circuit as GraphicsArray;
					if(graphicsArray != null) {
						this.Edit(graphicsArray);
						return;
					}
				} else if(this.CircuitRunner != null && this.CircuitRunner.VisibleMap != null) {
					CircuitMap map = this.CircuitRunner.VisibleMap.Child(circuitSymbol);
					if(map != null) {
						this.OpenLogicalCircuit(map);
						return;
					}
					if(circuitSymbol.Circuit is CircuitProbe) {
						FunctionProbe functionProbe = this.CircuitRunner.VisibleMap.FunctionProbe(circuitSymbol);
						if(functionProbe != null) {
							this.Mainframe.ShowDialog(new DialogProbeHistory(functionProbe));
						}
						return;
					}
					if((circuitSymbol.Circuit is Memory) || (circuitSymbol.Circuit is GraphicsArray)) {
						IFunctionMemory functionMemory = this.CircuitRunner.VisibleMap.FunctionMemory(circuitSymbol);
						if(functionMemory != null) {
							this.Mainframe.ShowDialog(new DialogMemory(functionMemory));
						}
						return;
					}
					if(circuitSymbol.Circuit is Constant) {
						if(this.CircuitRunner.Root.First() == this.CircuitRunner.VisibleMap) {
							FunctionConstant functionConstant = this.CircuitRunner.VisibleMap.FunctionConstant(circuitSymbol);
							if(functionConstant != null) {
								this.CircuitProject.InOmitTransaction(() => functionConstant.Value++);
							}
						} else {
							this.Mainframe.Status = Properties.Resources.MessageNotRootConstant(this.CircuitRunner.Root.First().Circuit.Name);
						}
					}
				}
			} else if(this.InEditMode) {
				TextNote textNote = symbol as TextNote;
				if(textNote != null) {
					this.Edit(textNote);
				}
			}
		}

		public void SelectAll() {
			if(this.InEditMode) {
				LogicalCircuit logicalCircuit = this.Project.LogicalCircuit;
				foreach(CircuitSymbol symbol in logicalCircuit.CircuitSymbols()) {
					this.Select(symbol);
				}
				foreach(Wire wire in logicalCircuit.Wires()) {
					this.Select(wire);
				}
				foreach(TextNote symbol in logicalCircuit.TextNotes()) {
					this.Select(symbol);
				}
			}
		}

		public void SelectAllWires() {
			if(this.InEditMode) {
				foreach(Wire wire in this.Project.LogicalCircuit.Wires()) {
					this.Select(wire);
				}
			}
		}

		public int SelectFreeWires() {
			if(this.InEditMode) {
				LogicalCircuit logicalCircuit = this.Project.LogicalCircuit;
				Dictionary<GridPoint, int> pointCount = new Dictionary<GridPoint, int>();
				Dictionary<GridPoint, Wire> firstWire = new Dictionary<GridPoint, Wire>();
				foreach(Wire wire in logicalCircuit.Wires()) {
					Tracer.Assert(wire.Point1 != wire.Point2);
					int count;
					if(pointCount.TryGetValue(wire.Point1, out count)) {
						if(count < 2) {
							pointCount[wire.Point1] = count + 1;
						}
					} else {
						pointCount.Add(wire.Point1, 1);
						firstWire.Add(wire.Point1, wire);
					}
					if(pointCount.TryGetValue(wire.Point2, out count)) {
						if(count < 2) {
							pointCount[wire.Point2] = count + 1;
						}
					} else {
						pointCount.Add(wire.Point2, 1);
						firstWire.Add(wire.Point2, wire);
					}
				}
				foreach(CircuitSymbol symbol in logicalCircuit.CircuitSymbols()) {
					foreach(Jam jam in symbol.Jams()) {
						int count;
						if(pointCount.TryGetValue(jam.AbsolutePoint, out count) && count < 2) {
							pointCount[jam.AbsolutePoint] = count + 1;
						}
					}
				}
				int freeWireCount = 0;
				foreach(KeyValuePair<GridPoint, int> pair in pointCount) {
					if(pair.Value < 2) {
						this.Select(firstWire[pair.Key]);
						freeWireCount++;
					}
				}
				return freeWireCount;
			}
			return 0;
		}

		public int SelectFloatingSymbols() {
			if(this.InEditMode) {
				LogicalCircuit logicalCircuit = this.Project.LogicalCircuit;
				HashSet<GridPoint> wirePoint = new HashSet<GridPoint>();
				foreach(Wire wire in logicalCircuit.Wires()) {
					wirePoint.Add(wire.Point1);
					wirePoint.Add(wire.Point2);
				}
				int count = 0;
				foreach(CircuitSymbol symbol in logicalCircuit.CircuitSymbols()) {
					foreach(Jam jam in symbol.Jams()) {
						if(!wirePoint.Contains(jam.AbsolutePoint)) {
							this.Select(symbol);
							count++;
							break;
						}
					}
				}
				return count;
			}
			return 0;
		}

		public void SelectAllButWires() {
			if(this.InEditMode) {
				foreach(Symbol symbol in this.Project.LogicalCircuit.CircuitSymbols()) {
					this.Select(symbol);
				}
				foreach(Symbol symbol in this.Project.LogicalCircuit.TextNotes()) {
					this.Select(symbol);
				}
			}
		}

		public void UnselectAllWires() {
			if(this.InEditMode) {
				foreach(Wire wire in this.Project.LogicalCircuit.Wires()) {
					this.Unselect(wire);
				}
			}
		}

		public void UnselectAllButWires() {
			if(this.InEditMode) {
				foreach(Symbol symbol in this.Project.LogicalCircuit.CircuitSymbols()) {
					this.Unselect(symbol);
				}
				foreach(Symbol symbol in this.Project.LogicalCircuit.TextNotes()) {
					this.Unselect(symbol);
				}
			}
		}

		public void SelectAllProbes(bool withWire) {
			if(this.InEditMode) {
				LogicalCircuit logicalCircuit = this.Project.LogicalCircuit;
				foreach(CircuitSymbol symbol in logicalCircuit.CircuitSymbols()) {
					if(symbol.Circuit is CircuitProbe) {
						this.Select(symbol);
						if(withWire) {
							Tracer.Assert(symbol.Jams().Count() == 1);
							GridPoint point = symbol.Jams().First().AbsolutePoint;
							foreach(Wire wire in logicalCircuit.Wires()) {
								if(wire.Point1 == point || wire.Point2 == point) {
									this.Select(wire);
								}
							}
						}
					}
				}
			}
		}

		public void Find() {
			this.Mainframe.ShowDialog(new DialogFind(this));
		}

		//--- Event Handling ---

		public void DiagramLostFocus() {
			this.CancelMove();
		}

		public void DiagramKeyDown(KeyEventArgs e) {
			if(this.InEditMode) {
				if(e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl) {
					this.switcher.OnControlDown();
					this.Mainframe.Status = Properties.Resources.TipOnCtrlDown;
					e.Handled = true;
				} else if(e.Key == Key.Tab) {
					this.switcher.OnTabDown(
						(Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.None,
						(Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.None
					);
					e.Handled = true;
				} else if(e.Key == Key.Escape) {
					this.CancelMove();
				}
			}
		}

		public void DiagramKeyUp(KeyEventArgs e) {
			if(this.InEditMode && (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)) {
				this.switcher.OnControlUp();
				e.Handled = true;
			}
		}

		public void DescriptorMouseDown(FrameworkElement sender, MouseButtonEventArgs e) {
			if(e.ChangedButton == MouseButton.Left && this.InEditMode) {
				IDescriptor descriptor = sender.DataContext as IDescriptor;
				if(descriptor != null) {
					if(1 < e.ClickCount) {
						LogicalCircuitDescriptor logicalCircuitDescriptor = descriptor as LogicalCircuitDescriptor;
						if(logicalCircuitDescriptor != null && !logicalCircuitDescriptor.IsCurrent) {
							this.OpenLogicalCircuit(logicalCircuitDescriptor.Circuit);
						}
					} else {
						this.dragStart = e.GetPosition(sender);
						this.dragSource = sender;
					}
				}
			}
		}

		public void DescriptorMouseUp() {
			this.dragSource = null;
		}

		public void DescriptorMouseMove(FrameworkElement sender, MouseEventArgs e) {
			if(this.InEditMode && this.dragSource != null) {
				Point point = e.GetPosition(this.dragSource);
				double x = point.X - this.dragStart.X;
				double y = point.Y - this.dragStart.Y;
				if(Editor.DragStartProximity < x * x + y * y) {
					this.dragSource = null;
					DragDrop.DoDragDrop(sender,
						new DataObject(EditorDiagram.CircuitDescriptorDataFormat, sender.DataContext),
						DragDropEffects.Copy | DragDropEffects.Scroll
					);
				}
			}
		}
	}
}

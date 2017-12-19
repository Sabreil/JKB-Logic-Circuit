﻿// This file is generated by ItemWrapper.Generator. Do not modify this file as it will be regenerated
namespace LogicCircuit {
	using System;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Globalization;
	using System.Linq;
	using System.Text;
	using LogicCircuit.DataPersistent;

	// Defines the shape of the table CircuitSymbol
	internal partial struct CircuitSymbolData {
		public Guid CircuitSymbolId;
		public Guid CircuitId;
		public Guid LogicalCircuitId;
		public int X;
		public int Y;
		public Rotation Rotation;
		internal CircuitSymbol CircuitSymbol;
		// Field accessors
		// Accessor of the CircuitSymbolId field
		public sealed class CircuitSymbolIdField : IField<CircuitSymbolData, Guid>, IFieldSerializer<CircuitSymbolData> {
			public static readonly CircuitSymbolIdField Field = new CircuitSymbolIdField();
			private CircuitSymbolIdField() {}
			public string Name { get { return "CircuitSymbolId"; } }
			public int Order { get; set; }
			public Guid DefaultValue { get { return default(Guid); } }
			public Guid GetValue(ref CircuitSymbolData record) {
				return record.CircuitSymbolId;
			}
			public void SetValue(ref CircuitSymbolData record, Guid value) {
				record.CircuitSymbolId = value;
			}
			public int Compare(ref CircuitSymbolData l, ref CircuitSymbolData r) {
				return l.CircuitSymbolId.CompareTo(r.CircuitSymbolId);
			}
			public int Compare(Guid l, Guid r) {
				return l.CompareTo(r);
			}

			// Implementation of interface IFieldSerializer<CircuitSymbolData>
			bool IFieldSerializer<CircuitSymbolData>.NeedToSave(ref CircuitSymbolData data) {
				return this.Compare(data.CircuitSymbolId, this.DefaultValue) != 0;
			}
			string IFieldSerializer<CircuitSymbolData>.GetTextValue(ref CircuitSymbolData data) {
				return string.Format(CultureInfo.InvariantCulture, "{0}", data.CircuitSymbolId);
			}
			void IFieldSerializer<CircuitSymbolData>.SetDefault(ref CircuitSymbolData data) {
				data.CircuitSymbolId = this.DefaultValue;
			}
			void IFieldSerializer<CircuitSymbolData>.SetTextValue(ref CircuitSymbolData data, string text) {
				data.CircuitSymbolId = new Guid(text);
			}
		}

		// Accessor of the CircuitId field
		public sealed class CircuitIdField : IField<CircuitSymbolData, Guid>, IFieldSerializer<CircuitSymbolData> {
			public static readonly CircuitIdField Field = new CircuitIdField();
			private CircuitIdField() {}
			public string Name { get { return "CircuitId"; } }
			public int Order { get; set; }
			public Guid DefaultValue { get { return default(Guid); } }
			public Guid GetValue(ref CircuitSymbolData record) {
				return record.CircuitId;
			}
			public void SetValue(ref CircuitSymbolData record, Guid value) {
				record.CircuitId = value;
			}
			public int Compare(ref CircuitSymbolData l, ref CircuitSymbolData r) {
				return l.CircuitId.CompareTo(r.CircuitId);
			}
			public int Compare(Guid l, Guid r) {
				return l.CompareTo(r);
			}

			// Implementation of interface IFieldSerializer<CircuitSymbolData>
			bool IFieldSerializer<CircuitSymbolData>.NeedToSave(ref CircuitSymbolData data) {
				return this.Compare(data.CircuitId, this.DefaultValue) != 0;
			}
			string IFieldSerializer<CircuitSymbolData>.GetTextValue(ref CircuitSymbolData data) {
				return string.Format(CultureInfo.InvariantCulture, "{0}", data.CircuitId);
			}
			void IFieldSerializer<CircuitSymbolData>.SetDefault(ref CircuitSymbolData data) {
				data.CircuitId = this.DefaultValue;
			}
			void IFieldSerializer<CircuitSymbolData>.SetTextValue(ref CircuitSymbolData data, string text) {
				data.CircuitId = new Guid(text);
			}
		}

		// Accessor of the LogicalCircuitId field
		public sealed class LogicalCircuitIdField : IField<CircuitSymbolData, Guid>, IFieldSerializer<CircuitSymbolData> {
			public static readonly LogicalCircuitIdField Field = new LogicalCircuitIdField();
			private LogicalCircuitIdField() {}
			public string Name { get { return "LogicalCircuitId"; } }
			public int Order { get; set; }
			public Guid DefaultValue { get { return default(Guid); } }
			public Guid GetValue(ref CircuitSymbolData record) {
				return record.LogicalCircuitId;
			}
			public void SetValue(ref CircuitSymbolData record, Guid value) {
				record.LogicalCircuitId = value;
			}
			public int Compare(ref CircuitSymbolData l, ref CircuitSymbolData r) {
				return l.LogicalCircuitId.CompareTo(r.LogicalCircuitId);
			}
			public int Compare(Guid l, Guid r) {
				return l.CompareTo(r);
			}

			// Implementation of interface IFieldSerializer<CircuitSymbolData>
			bool IFieldSerializer<CircuitSymbolData>.NeedToSave(ref CircuitSymbolData data) {
				return this.Compare(data.LogicalCircuitId, this.DefaultValue) != 0;
			}
			string IFieldSerializer<CircuitSymbolData>.GetTextValue(ref CircuitSymbolData data) {
				return string.Format(CultureInfo.InvariantCulture, "{0}", data.LogicalCircuitId);
			}
			void IFieldSerializer<CircuitSymbolData>.SetDefault(ref CircuitSymbolData data) {
				data.LogicalCircuitId = this.DefaultValue;
			}
			void IFieldSerializer<CircuitSymbolData>.SetTextValue(ref CircuitSymbolData data, string text) {
				data.LogicalCircuitId = new Guid(text);
			}
		}

		// Accessor of the X field
		public sealed class XField : IField<CircuitSymbolData, int>, IFieldSerializer<CircuitSymbolData> {
			public static readonly XField Field = new XField();
			private XField() {}
			public string Name { get { return "X"; } }
			public int Order { get; set; }
			public int DefaultValue { get { return default(int); } }
			public int GetValue(ref CircuitSymbolData record) {
				return record.X;
			}
			public void SetValue(ref CircuitSymbolData record, int value) {
				record.X = value;
			}
			public int Compare(ref CircuitSymbolData l, ref CircuitSymbolData r) {
				return Math.Sign((long)l.X - (long)r.X);
			}
			public int Compare(int l, int r) {
				return Math.Sign((long)l - (long)r);
			}

			// Implementation of interface IFieldSerializer<CircuitSymbolData>
			bool IFieldSerializer<CircuitSymbolData>.NeedToSave(ref CircuitSymbolData data) {
				return this.Compare(data.X, this.DefaultValue) != 0;
			}
			string IFieldSerializer<CircuitSymbolData>.GetTextValue(ref CircuitSymbolData data) {
				return string.Format(CultureInfo.InvariantCulture, "{0}", data.X);
			}
			void IFieldSerializer<CircuitSymbolData>.SetDefault(ref CircuitSymbolData data) {
				data.X = this.DefaultValue;
			}
			void IFieldSerializer<CircuitSymbolData>.SetTextValue(ref CircuitSymbolData data, string text) {
				data.X = int.Parse(text, CultureInfo.InvariantCulture);
			}
		}

		// Accessor of the Y field
		public sealed class YField : IField<CircuitSymbolData, int>, IFieldSerializer<CircuitSymbolData> {
			public static readonly YField Field = new YField();
			private YField() {}
			public string Name { get { return "Y"; } }
			public int Order { get; set; }
			public int DefaultValue { get { return default(int); } }
			public int GetValue(ref CircuitSymbolData record) {
				return record.Y;
			}
			public void SetValue(ref CircuitSymbolData record, int value) {
				record.Y = value;
			}
			public int Compare(ref CircuitSymbolData l, ref CircuitSymbolData r) {
				return Math.Sign((long)l.Y - (long)r.Y);
			}
			public int Compare(int l, int r) {
				return Math.Sign((long)l - (long)r);
			}

			// Implementation of interface IFieldSerializer<CircuitSymbolData>
			bool IFieldSerializer<CircuitSymbolData>.NeedToSave(ref CircuitSymbolData data) {
				return this.Compare(data.Y, this.DefaultValue) != 0;
			}
			string IFieldSerializer<CircuitSymbolData>.GetTextValue(ref CircuitSymbolData data) {
				return string.Format(CultureInfo.InvariantCulture, "{0}", data.Y);
			}
			void IFieldSerializer<CircuitSymbolData>.SetDefault(ref CircuitSymbolData data) {
				data.Y = this.DefaultValue;
			}
			void IFieldSerializer<CircuitSymbolData>.SetTextValue(ref CircuitSymbolData data, string text) {
				data.Y = int.Parse(text, CultureInfo.InvariantCulture);
			}
		}

		// Accessor of the Rotation field
		public sealed class RotationField : IField<CircuitSymbolData, Rotation>, IFieldSerializer<CircuitSymbolData> {
			public static readonly RotationField Field = new RotationField();
			private RotationField() {}
			public string Name { get { return "Rotation"; } }
			public int Order { get; set; }
			public Rotation DefaultValue { get { return Rotation.Up; } }
			public Rotation GetValue(ref CircuitSymbolData record) {
				return record.Rotation;
			}
			public void SetValue(ref CircuitSymbolData record, Rotation value) {
				record.Rotation = value;
			}
			public int Compare(ref CircuitSymbolData l, ref CircuitSymbolData r) {
				return l.Rotation.CompareTo(r.Rotation);
			}
			public int Compare(Rotation l, Rotation r) {
				return l.CompareTo(r);
			}

			// Implementation of interface IFieldSerializer<CircuitSymbolData>
			bool IFieldSerializer<CircuitSymbolData>.NeedToSave(ref CircuitSymbolData data) {
				return this.Compare(data.Rotation, this.DefaultValue) != 0;
			}
			string IFieldSerializer<CircuitSymbolData>.GetTextValue(ref CircuitSymbolData data) {
				return string.Format(CultureInfo.InvariantCulture, "{0}", data.Rotation);
			}
			void IFieldSerializer<CircuitSymbolData>.SetDefault(ref CircuitSymbolData data) {
				data.Rotation = this.DefaultValue;
			}
			void IFieldSerializer<CircuitSymbolData>.SetTextValue(ref CircuitSymbolData data, string text) {
				data.Rotation = EnumHelper.Parse<Rotation>(text, this.DefaultValue);
			}
		}

		// Special field used to access items wrapper of this record from record.
		// This is used when no other universes is used
		internal sealed class CircuitSymbolField : IField<CircuitSymbolData, CircuitSymbol> {
			public static readonly CircuitSymbolField Field = new CircuitSymbolField();
			private CircuitSymbolField() {}
			public string Name { get { return "CircuitSymbolWrapper"; } }
			public int Order { get; set; }
			public CircuitSymbol DefaultValue { get { return null; } }
			public CircuitSymbol GetValue(ref CircuitSymbolData record) {
				return record.CircuitSymbol;
			}
			public void SetValue(ref CircuitSymbolData record, CircuitSymbol value) {
				record.CircuitSymbol = value;
			}
			public int Compare(ref CircuitSymbolData l, ref CircuitSymbolData r) {
				return this.Compare(l.CircuitSymbol, r.CircuitSymbol);
			}
			public int Compare(CircuitSymbol l, CircuitSymbol r) {
				if(object.ReferenceEquals(l, r)) return 0;
				if(l == null) return -1;
				if(r == null) return 1;
				return l.CircuitSymbolRowId.CompareTo(r.CircuitSymbolRowId);
			}
		}

		private static IField<CircuitSymbolData>[] fields = {
			CircuitSymbolIdField.Field,
			CircuitIdField.Field,
			LogicalCircuitIdField.Field,
			XField.Field,
			YField.Field,
			RotationField.Field,
			CircuitSymbolField.Field
		};

		// Creates table.
		public static TableSnapshot<CircuitSymbolData> CreateTable(StoreSnapshot store) {
			TableSnapshot<CircuitSymbolData> table = new TableSnapshot<CircuitSymbolData>(store, "CircuitSymbol", CircuitSymbolData.fields);
			// Create all but foreign keys of the table
			table.MakeUnique("PK_CircuitSymbol", CircuitSymbolData.CircuitSymbolIdField.Field , true);
			table.CreateIndex("IX_Circuit_CircuitSymbol", CircuitSymbolData.CircuitIdField.Field );
			table.CreateIndex("IX_LogicalCircuit_CircuitSymbol", CircuitSymbolData.LogicalCircuitIdField.Field );
			// Return created table
			return table;
		}

		// Creates all foreign keys of the table
		public static void CreateForeignKeys(StoreSnapshot store) {
			TableSnapshot<CircuitSymbolData> table = (TableSnapshot<CircuitSymbolData>)store.Table("CircuitSymbol");
			table.CreateForeignKey("FK_Circuit_CircuitSymbol", store.Table("Circuit"), CircuitSymbolData.CircuitIdField.Field, ForeignKeyAction.Cascade, false);
			table.CreateForeignKey("FK_LogicalCircuit_CircuitSymbol", store.Table("LogicalCircuit"), CircuitSymbolData.LogicalCircuitIdField.Field, ForeignKeyAction.Restrict, false);
		}
	}

	// Class wrapper for a record.
	partial class CircuitSymbol : CircuitGlyph {

		// RowId of the wrapped record
		internal RowId CircuitSymbolRowId { get; private set; }
		// Store this wrapper belongs to
		public CircuitProject CircuitProject { get; private set; }

		// Constructor
		public CircuitSymbol(CircuitProject store, RowId rowIdCircuitSymbol) {
			Debug.Assert(!rowIdCircuitSymbol.IsEmpty);
			this.CircuitProject = store;
			this.CircuitSymbolRowId = rowIdCircuitSymbol;
			// Link back to record. Assuming that a transaction is started
			this.Table.SetField(this.CircuitSymbolRowId, CircuitSymbolData.CircuitSymbolField.Field, this);
			this.InitializeCircuitSymbol();
		}

		partial void InitializeCircuitSymbol();

		// Gets table storing this item.
		private TableSnapshot<CircuitSymbolData> Table { get { return this.CircuitProject.CircuitSymbolSet.Table; } }

		// Deletes object
		public virtual void Delete() {
			this.Table.Delete(this.CircuitSymbolRowId);
		}

		// Checks if the item is deleted
		public bool IsDeleted() {
			return this.Table.IsDeleted(this.CircuitSymbolRowId);
		}

		//Properties of CircuitSymbol

		// Gets value of the CircuitSymbolId field.
		public Guid CircuitSymbolId {
			get { return this.Table.GetField(this.CircuitSymbolRowId, CircuitSymbolData.CircuitSymbolIdField.Field); }
		}

		// Gets the value referred by the foreign key on field CircuitId
		protected override Circuit SymbolCircuit {
			get { return this.CircuitProject.CircuitSet.Find(this.Table.GetField(this.CircuitSymbolRowId, CircuitSymbolData.CircuitIdField.Field)); }
		}

		// Gets or sets the value referred by the foreign key on field LogicalCircuitId
		protected override LogicalCircuit SymbolLogicalCircuit {
			get { return this.CircuitProject.LogicalCircuitSet.FindByLogicalCircuitId(this.Table.GetField(this.CircuitSymbolRowId, CircuitSymbolData.LogicalCircuitIdField.Field)); }
			set { this.Table.SetField(this.CircuitSymbolRowId, CircuitSymbolData.LogicalCircuitIdField.Field, value.LogicalCircuitId); }
		}

		// Gets or sets value of the X field.
		public int X {
			get { return this.Table.GetField(this.CircuitSymbolRowId, CircuitSymbolData.XField.Field); }
			set { this.Table.SetField(this.CircuitSymbolRowId, CircuitSymbolData.XField.Field, value); }
		}

		// Gets or sets value of the Y field.
		public int Y {
			get { return this.Table.GetField(this.CircuitSymbolRowId, CircuitSymbolData.YField.Field); }
			set { this.Table.SetField(this.CircuitSymbolRowId, CircuitSymbolData.YField.Field, value); }
		}

		// Gets or sets value of the Rotation field.
		public Rotation Rotation {
			get { return this.Table.GetField(this.CircuitSymbolRowId, CircuitSymbolData.RotationField.Field); }
			set { this.Table.SetField(this.CircuitSymbolRowId, CircuitSymbolData.RotationField.Field, value); }
		}


		internal void NotifyChanged(TableChange<CircuitSymbolData> change) {
			if(this.HasListener) {
				CircuitSymbolData oldData, newData;
				change.GetOldData(out oldData);
				change.GetNewData(out newData);
				if(CircuitSymbolData.CircuitSymbolIdField.Field.Compare(ref oldData, ref newData) != 0) {
					this.NotifyPropertyChanged("CircuitSymbolId");
				}
				if(CircuitSymbolData.CircuitIdField.Field.Compare(ref oldData, ref newData) != 0) {
					this.NotifyPropertyChanged("Circuit");
				}
				if(CircuitSymbolData.LogicalCircuitIdField.Field.Compare(ref oldData, ref newData) != 0) {
					this.NotifyPropertyChanged("LogicalCircuit");
				}
				if(CircuitSymbolData.XField.Field.Compare(ref oldData, ref newData) != 0) {
					this.NotifyPropertyChanged("X");
				}
				if(CircuitSymbolData.YField.Field.Compare(ref oldData, ref newData) != 0) {
					this.NotifyPropertyChanged("Y");
				}
				if(CircuitSymbolData.RotationField.Field.Compare(ref oldData, ref newData) != 0) {
					this.NotifyPropertyChanged("Rotation");
				}
			}
			this.OnCircuitSymbolChanged();
		}

		partial void OnCircuitSymbolChanged();
	}


	// Wrapper for table CircuitSymbol.
	partial class CircuitSymbolSet : INotifyCollectionChanged, IEnumerable<CircuitSymbol> {

		public event NotifyCollectionChangedEventHandler CollectionChanged;

		internal TableSnapshot<CircuitSymbolData> Table { get; private set; }

		// Gets StoreSnapshot this set belongs to.
		public CircuitProject CircuitProject { get { return (CircuitProject)this.Table.StoreSnapshot; } }

		// Constructor
		public CircuitSymbolSet(CircuitProject store) {
			ITableSnapshot table = store.Table("CircuitSymbol");
			if(table != null) {
				Debug.Assert(store.IsFrozen, "The store should be frozen");
				this.Table = (TableSnapshot<CircuitSymbolData>)table;
			} else {
				Debug.Assert(!store.IsFrozen, "In order to create table, the store should not be frozen");
				this.Table = CircuitSymbolData.CreateTable(store);
			}
			this.InitializeCircuitSymbolSet();
		}

		partial void InitializeCircuitSymbolSet();

		//internal void Register() {
		//	foreach(RowId rowId in this.Table.Rows) {
		//		this.FindOrCreate(rowId);
		//	}
		//}


		// gets items wrapper by RowId
		public CircuitSymbol Find(RowId rowId) {
			if(!rowId.IsEmpty) {
				return this.Table.GetField(rowId, CircuitSymbolData.CircuitSymbolField.Field);
			}
			return null;
		}


		// gets items wrapper by RowId
		private IEnumerable<CircuitSymbol> Select(IEnumerable<RowId> rows) {
			foreach(RowId rowId in rows) {
				CircuitSymbol item = this.Find(rowId);
				Debug.Assert(item != null, "What is the reason for the item not to be found?");
				yield return item;
			}
		}

		// Create wrapper for the row and register it in the dictionary
		private CircuitSymbol Create(RowId rowId) {
			CircuitSymbol item = new CircuitSymbol(this.CircuitProject, rowId);
			return item;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
		internal CircuitSymbol FindOrCreate(RowId rowId) {
			Debug.Assert(!rowId.IsEmpty && !this.Table.IsDeleted(rowId), "Bad RowId");
			CircuitSymbol item;
			if((item = this.Find(rowId)) != null) {
				Debug.Assert(!item.IsDeleted(), "Deleted item should not be present in the dictionary");
				return item;
			}

			return this.Create(rowId);
		}

		// Creates CircuitSymbol wrapper
		private CircuitSymbol CreateItem(
			// Fields of CircuitSymbol table
			Guid CircuitSymbolId,
			Circuit Circuit,
			LogicalCircuit LogicalCircuit,
			int X,
			int Y,
			Rotation Rotation
		) {
			CircuitSymbolData dataCircuitSymbol = new CircuitSymbolData() {
				CircuitSymbolId = CircuitSymbolId,
				CircuitId = (Circuit != null) ? Circuit.CircuitId : CircuitSymbolData.CircuitIdField.Field.DefaultValue,
				LogicalCircuitId = (LogicalCircuit != null) ? LogicalCircuit.LogicalCircuitId : CircuitSymbolData.LogicalCircuitIdField.Field.DefaultValue,
				X = X,
				Y = Y,
				Rotation = Rotation,
			};
			return this.Create(this.Table.Insert(ref dataCircuitSymbol));
		}

		// Search helpers

		// Finds CircuitSymbol by CircuitSymbolId
		public CircuitSymbol Find(Guid circuitSymbolId) {
			return this.Find(this.Table.Find(CircuitSymbolData.CircuitSymbolIdField.Field, circuitSymbolId));
		}

		// Selects CircuitSymbol by Circuit
		public IEnumerable<CircuitSymbol> SelectByCircuit(Circuit circuit) {
			return this.Select(this.Table.Select(CircuitSymbolData.CircuitIdField.Field, circuit.CircuitId));
		}

		// Selects CircuitSymbol by LogicalCircuit
		public IEnumerable<CircuitSymbol> SelectByLogicalCircuit(LogicalCircuit logicalCircuit) {
			return this.Select(this.Table.Select(CircuitSymbolData.LogicalCircuitIdField.Field, logicalCircuit.LogicalCircuitId));
		}

		public IEnumerator<CircuitSymbol> GetEnumerator() {
			return this.Select(this.Table.Rows).GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return this.GetEnumerator();
		}

		private void NotifyCollectionChanged(NotifyCollectionChangedEventArgs arg) {
			NotifyCollectionChangedEventHandler handler = this.CollectionChanged;
			if(handler != null) {
				handler(this, arg);
			}
		}

		internal List<CircuitSymbol> UpdateSet(int oldVersion, int newVersion) {
			IEnumerator<TableChange<CircuitSymbolData>> change = this.Table.GetVersionChangeChanges(oldVersion, newVersion);
			if(change != null) {
				bool handlerAttached = (this.CollectionChanged != null);
				List<CircuitSymbol> del = (handlerAttached) ? new List<CircuitSymbol>() : null;
				while(change.MoveNext()) {
					switch(change.Current.Action) {
					case SnapTableAction.Insert:
						this.FindOrCreate(change.Current.RowId);
						Debug.Assert(this.Find(change.Current.RowId) != null, "Why the item was not created?");
						break;
					case SnapTableAction.Delete:
						if(handlerAttached) {
							CircuitSymbol item = change.Current.GetOldField(CircuitSymbolData.CircuitSymbolField.Field);
							Debug.Assert(item.IsDeleted());
							del.Add(item);
						}
						break;
					default:
						Debug.Assert(change.Current.Action == SnapTableAction.Update, "Unknown action");
						Debug.Assert(this.Find(change.Current.RowId) != null, "Why the item does not exist during update?");
						break;
					}
				}
				change.Dispose();
				return del;
			}
			return null;
		}

		internal void NotifyVersionChanged(int oldVersion, int newVersion, List<CircuitSymbol> deleted) {
			IEnumerator<TableChange<CircuitSymbolData>> change = this.Table.GetVersionChangeChanges(oldVersion, newVersion);
			if(change != null) {
				bool handlerAttached = (this.CollectionChanged != null);
				List<CircuitSymbol> add = (handlerAttached) ? new List<CircuitSymbol>() : null;
				this.StartNotifyCircuitSymbolSetChanged(oldVersion, newVersion);
				while(change.MoveNext()) {
					this.NotifyCircuitSymbolSetChanged(change.Current);
					switch(change.Current.Action) {
					case SnapTableAction.Insert:
						Debug.Assert(this.Find(change.Current.RowId) != null, "Why the item was not created?");
						if(handlerAttached) {
							add.Add(this.Find(change.Current.RowId));
						}
						break;
					case SnapTableAction.Delete:
						Debug.Assert(change.Current.GetOldField(CircuitSymbolData.CircuitSymbolField.Field).IsDeleted(), "Why the item still exists?");
						break;
					default:
						Debug.Assert(change.Current.Action == SnapTableAction.Update, "Unknown action");
						Debug.Assert(this.Find(change.Current.RowId) != null, "Why the item does not exist during update?");
						this.Find(change.Current.RowId).NotifyChanged(change.Current);
						break;
					}
				}
				change.Dispose();
				if(handlerAttached) {
					if(deleted != null && 0 < deleted.Count) {
						this.NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, deleted));
					}
					if(0 < add.Count) {
						this.NotifyCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, add));
					}
				}
				this.EndNotifyCircuitSymbolSetChanged();
			}
		}

		partial void StartNotifyCircuitSymbolSetChanged(int oldVersion, int newVersion);
		partial void EndNotifyCircuitSymbolSetChanged();
		partial void NotifyCircuitSymbolSetChanged(TableChange<CircuitSymbolData> change);

		internal void NotifyRolledBack(int version) {
			if(this.Table.WasAffected(version)) {
				IEnumerator<RowId> change = this.Table.GetRolledBackChanges(version);
				if(change != null) {
					while(change.MoveNext()) {
						RowId rowId = change.Current;
						if(this.Table.IsDeleted(rowId)) {
						} else {
							this.FindOrCreate(rowId);
						}
					}
					change.Dispose();
				}
			}
		}
	}

}
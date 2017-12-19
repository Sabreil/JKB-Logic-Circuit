using System;
// jkb: add these two
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;

namespace LogicCircuit {
	public class FunctionRam : FunctionMemory {

        //jkb
        [SuppressMessage("Microsoft.Usage", "CA2211: Non-constant fields should not be visible")]
        public static FunctionRam keyboardRAM;
        private bool shiftKeyPressed; // this is here so we can remember the shift key for Keyboard emulation
        private bool capsLockPressed = false; // this is here so we can remember the caps lock key for Keyboard emulation
        private bool sawKeyNoneAfterCapsLock = true;
        // jkb end

        public FunctionRam(CircuitState circuitState, int[] address, int[] inputData, int[] outputData, int write, Memory memory) : base(
			circuitState, address, inputData, outputData, write, memory)
        {
            // jkb: remember this guy if its a keyboard
            if (this.Memory.MapKeyboard == MemoryMapKeyboard.Hack) keyboardRAM = this;
        }

		public override bool Evaluate() {
			if(this.IsWriteAllowed()) {
				this.Write();
			}
			return this.Read();
		}

		public override string ReportName { get { return Properties.Resources.ReportMemoryName(Properties.Resources.RAMNotation, this.AddressBitWidth, this.DataBitWidth); } }

        // jkb
        public bool WriteKeyValue(Key keyBeingPressed)
        {
            // if keyboard sim is enabled set, put the ASCII value of the key being pressed into memory
            // at location zero (the address memory-mapped to keyboard input)
            // return true if all OK
            if (keyBeingPressed == Key.None)
            {
                sawKeyNoneAfterCapsLock = true;
                if (shiftKeyPressed) shiftKeyPressed = false;
                if (this.Memory.MapKeyboard == MemoryMapKeyboard.Hack)
                {
                    Memory.SetCellValue(this.data, 16, 0, 0);
                    return (true);
                }
            }

            short rawKeyValue = (short)keyBeingPressed;
            short ASCIIKeyValue = 0;

            if (keyBeingPressed == Key.Capital) // raw code 0x0008
            {
                if (sawKeyNoneAfterCapsLock)
                {
                    capsLockPressed = !capsLockPressed;
                    sawKeyNoneAfterCapsLock = false;
                }
            }
            if ((keyBeingPressed == Key.LeftShift) || (keyBeingPressed == Key.RightShift))
            {
                shiftKeyPressed = true; // remember while some other key is pressed 
            }

            // Translate the Key enumeration into ASCII
            // Take the hex code the keyboard gives us, and depending upon whether the shift key was just pressed, 
            // output the correct ASCII code for the key.  In some cases, we use the Hack Computer book settings 
            // (see page 91 of Nisan and Schocken).
            // We use a custom dictionary lookup to determine the value

            // If different keyboards use different key to hex mappings, we will need custom dictionaries for each.
            // All unmapped values return 0

            // add 0x100 to the raw code if shifted or caps-locked.
            if (shiftKeyPressed || capsLockPressed)
            {
                rawKeyValue += 0x100;
            }

            if (KeyMapTable.keyMap.ContainsKey(rawKeyValue))
            {
                ASCIIKeyValue = KeyMapTable.keyMap[rawKeyValue];
            }
            else ASCIIKeyValue = 0;

            if (this.Memory.MapKeyboard == MemoryMapKeyboard.Hack)
            {
                Memory.SetCellValue(this.data, 16, 0, ASCIIKeyValue);
                return (true);
            }
            return (false);
        }
        // end jkb

    }
}

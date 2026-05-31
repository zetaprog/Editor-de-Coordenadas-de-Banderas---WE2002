
## 📖 Overview
This tool loads a game's binary file, parses flag coordinate structures, and renders them on an interactive canvas. You can drag and drop flags to fix misalignments, with real-time coordinate feedback and strict boundary enforcement. Changes are saved directly back to the binary file.

## ✨ Features
- 🖱️ **Drag & Drop Interface**: Move flags visually with mouse input
-  **Color-Coded Flags**: Blue for National Teams, Red for Master League
- 📏 **Real-Time Tooltips**: Hover to see `ID`, `X`, and `Y` coordinates
-  **Boundary Enforcement**: Prevents flags from leaving the valid menu area
- 💾 **Direct Binary Patching**: Saves modified coordinates back to the original file
- 📐 **Y-Axis Inversion Handling**: Matches the game's upward-growing Y coordinate system
- ⚡ **Fast Parsing**: Scans predefined memory ranges for flag structures

## 🛠️ Requirements
- **OS**: Windows 7 / 8 / 10 / 11
- **Runtime**: .NET Framework 4.7.2 or higher
- **Target File**: `.exe`, `.bin`, or `.dat` file from a No$psx-compatible football game

## 🚀 Usage
1. **Launch** the application.
2. Click **Load** and select your game file.
3. **Drag flags** to the desired positions within the white bounding box.
   - Use the **white rectangle** as the menu boundary.
   - The **green dashed line** represents `Y = 0` (bottom of the menu).
4. Click **Save** to patch the file. A confirmation dialog will appear.
5. **Test** the modified file in the No$psx emulator.

> 💡 **Tip**: Double-click a flag to reset it to its original position (if the restore feature is enabled).

## ⚙️ Technical Details

### 🔍 Scan Ranges
| Section      | Start Offset | Scan Size | Description          |
|--------------|--------------|-----------|----------------------|
| National Teams | `0x28D228`   | 650 bytes | Country/Selection flags |
| Master League  | `0x28D4B2`   | 320 bytes | ML team flags        |

### 📦 Flag Structure (10 Bytes)
| Offset | Type     | Description        |
|--------|----------|--------------------|
| `+0`   | `Byte`   | Width              |
| `+1`   | `Byte`   | Height             |
| `+2`   | `Byte`   | Crop X             |
| `+3`   | `Byte`   | Crop Y             |
| `+4`   | `UShort` | Position X         |
| `+6`   | `Short`  | Position Y         |
| `+8`   | `Byte`   | Extra/Flags        |
| `+9`   | `Byte`   | ID                 |

### 📐 Coordinate System
- **Origin `(0,0)`**: Top-left corner of the canvas.
- **Y-Axis Direction**: The game engine uses an **upward-positive Y axis**. `Y=0` maps to the bottom green line in the editor. Positive values move flags upward.
- **Visual Offset**: A `-122px` vertical offset is applied during rendering to perfectly align the editor canvas with the emulator's final output.

## ⚠️ Disclaimer & Safety
1.  **Always backup** your original game file before editing. The developer is not responsible for corrupted files or emulator crashes.
2. 📏 **Do not move flags outside the white boundary**. Out-of-bounds coordinates may cause rendering glitches or crashes in-game.
3. 🛠️ This tool is intended for **personal, educational, and modding purposes only**. It does not distribute copyrighted game assets.

## 📜 License
This project is released under the **MIT License**. Feel free to use, modify, and distribute as long as proper attribution is given.

---
*Built with ❤️ using VB.NET & Windows Forms*

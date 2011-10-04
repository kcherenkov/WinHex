using System;
using System.Runtime.InteropServices;
using System.Text;

namespace WinHex
{
    public static class WinHex
    {
        #region WinHexAPI
        /// <summary>
        /// Must be called first to initialize the WinHex API. WinHex must not be running. If initialization fails, the WinHex API must not and cannot be used.
        /// </summary>
        /// <param name="APIVersion">APIVersion currently must be 1.</param>
        /// <returns>2		Success (limited)
        /// A return value of 2 indicates that the WinHex API may only be used for evaluation purposes (possible as of WinHex 10.8). This is the case if the evaluation version of WinHex or only a personal license is available. All the commands the WinHex API offers can be used then except WHX_Save, WHX_SaveAs, WHX_SaveAll, and WHX_Write, which will always fail.</returns>
        /// <returns>1		Success</returns>
        /// <returns>0		General error</returns>
        /// <returns>-1		WinHex installation not ready</returns>
        /// <returns>-2		APIVersion incorrect</returns>
        /// <returns>-3		Invalid or insufficient license
        /// The return value of -3 is no longer in use since WinHex 10.8.</returns>
        [DllImport("whxapi")]
        private static extern int WHX_Init(int APIVersion);

        /// <summary>
        /// Must be called when the WinHex API is no longer needed, to properly free all allocated resources and to terminate the active part of WinHex.
        /// </summary>
        [DllImport("whxapi")]
        private static extern bool WHX_Done();

        /// <summary>
        /// Opens the specified file, logical drive or physical disk in default edit mode. Under Windows NT/2000/XP, administrator privileges are required to open drives or disks. 
        /// </summary>
        /// <param name="lpResName">file, logical drive or physical disk</param>
        [DllImport("whxapi")]
        private static extern bool WHX_Open(string lpResName);

        /// <summary>
        /// Creates the specified file with the specified initial files size in the specified directory. If the file exists, it is overwritten. The newly created file is also opened. WinHex cannot create a file of 0 or more than 2^31-1 bytes.
        /// </summary>
        [DllImport("whxapi")]
        private static extern bool WHX_Create(string lpPathName, int Size);

        /// <summary>
        /// Closes the active file or disk. Any unsaved changes are lost.
        /// </summary>
        [DllImport("whxapi")]
        private static extern bool WHX_Close();

        /// <summary>
        /// Closes all open files or disks. Any unsaved changes are lost.
        /// </summary>
        [DllImport("whxapi")]
        private static extern bool WHX_CloseAll();

        /// <summary>
        /// Switches cyclically to the next open file or disk and makes it the “active” file or disk.
        /// </summary>
        /// <returns></returns>
        [DllImport("whxapi")]
        private static extern bool WHX_NextObj();

        /// <summary>
        /// Saves all changes to the active file or disk.
        /// </summary>
        [DllImport("whxapi")]
        private static extern bool WHX_Save();

        /// <summary>
        /// Save the active file under the specified name.
        /// </summary>
        [DllImport("whxapi")]
        private static extern bool WHX_SaveAs(string lpNewFileName);

        /// <summary>
        /// Saves all changes to all open files and disks.
        /// </summary>
        [DllImport("whxapi")]
        private static extern bool WHX_SaveAll();

        /// <summary>
        /// Works the same as WHXOpen, but passes an additional integer parameter Param that consists of flags. There are two mutually exclusive flags that determine in which edit mode the file or disk is to be opened. Using in-place edit mode accelerates writing (because data is written directly and immediately, without the use of temporary files) and renders calling WHX_Save obsolete. Read-only mode (view mode, write protection) ensures that nothing will be accidentally written. Available since WinHex 10.92 SR-2. There is another flag that causes files to be opened and treated like image files (applicable to raw images, Encase images, and evidence files). Available since WinHex 11.9.
        /// </summary>
        /// <param name="Param">
        /// 0x00000001: read-only instead of default edit mode
        /// 0x00000002: in-place instead of default edit mode
        /// 0x00000004: treat file like an image file (requires specialist or forensic license)
        /// </param>
        [DllImport("whxapi")]
        private static extern bool WHX_OpenEx(string lpResName, int Param);

        /// <summary>
        /// Reads Bytes bytes from the current position in the active file or disk into the specified buffer. Also moves the current position forward by Bytes bytes, provided the file or disk is sufficient in size.
        /// </summary>
        [DllImport("whxapi")]
        private static extern bool WHX_Read(byte[] lpBuffer, int Bytes);

        /// <summary>
        /// Writes Bytes bytes from the specified buffer at the current position into the active file or disk (in overwrite mode). Also moves the current position forward by Bytes bytes. Expands the file size if necessary to do this.
        /// </summary>
        [DllImport("whxapi")]
        private static extern bool WHX_Write(byte[] lpBuffer, int Bytes);

        /// <summary>
        /// Returns the total size of the file or disk in bytes as a 64-bit integer at the address specified by lpSize.
        /// </summary>
        [DllImport("whxapi")]
        private static extern bool WHX_GetSize(out long lpSize);

        /// <summary>
        /// Moves the current position to the specified 64-bit offset.
        /// </summary>
        [DllImport("whxapi")]
        private static extern bool WHX_Goto(long Ofs);

        /// <summary>
        /// Moves the current position by the specified 64-bit distance, forward (>0) or backward (<0).
        /// </summary>
        [DllImport("whxapi")]
        private static extern bool WHX_Move(long Distance);

        /// <summary>
        /// Returns the current position as a 64-bit offset at the address specified by lpOfs.
        /// </summary>
        [DllImport("whxapi")]
        private static extern bool WHX_CurrentPos(out long lpOfs);

        /// <summary>
        /// Sets the block borders to the specified offsets. To clear the currently selected block, Ofs1 and Ofs2 must both be -1 (new since WinHex 10.55 SR-4).
        /// </summary>
        [DllImport("whxapi")]
        private static extern bool WHX_SetBlock(long Ofs1, long Ofs2);

        /// <summary>
        /// Copies the currently defined block into the clipboard. If no block is defined, this function works as known from the Copy command in the Edit menu.
        /// </summary>
        [DllImport("whxapi")]
        private static extern bool WHX_Copy();

        /// <summary>
        /// Copies the currently defined block into the specified new file, without using the clipboard. If no block is defined, it works as known from the Copy command in the Edit menu. Can copy files as well as disk sectors (an easy way to create a disk image). The new file will not be opened automatically.
        /// </summary>
        [DllImport("whxapi")]
        private static extern bool WHX_CopyIntoNewFile(string lpNewFileName);

        /// <summary>
        /// Cuts the currently defined block from the file and puts it into the clipboard.
        /// </summary>
        [DllImport("whxapi")]
        private static extern bool WHX_Cut();

        /// <summary>
        /// Removes the currently defined block from the file.
        /// </summary>
        [DllImport("whxapi")]
        private static extern bool WHX_Remove();

        /// <summary>
        /// Pastes the current clipboard contents at the current position in a file, without changing the current position. 
        /// </summary>
        [DllImport("whxapi")]
        private static extern bool WHX_Paste();

        /// <summary>
        /// Writes the current clipboard contents at the current position in a file or disk, without changing the current position and by overwriting the data at the current position. 
        /// </summary>
        [DllImport("whxapi")]
        private static extern bool WHX_WriteClipboard();

        /// <summary>
        /// Searches for the data given by lpData. This may either be null-terminated raw data or a null-terminated string in hexadecimal ASCII notation (like “0x123456”). The function moves the current position to the first occurrence, if any.
        /// </summary>
        /// <param name="lpOptions">lpOptions points to a string that consists of any combination (concatenation) of search options. Supported options are “MatchCase”, “MatchWord”, “Down”, “Up”, “BlockOnly”, “SaveAllPos”, “Wildcards”, and “Unicode”. For example, a valid string would be “Down MatchCase BlockOnly”. By default, if the string is empty or the pointer is null, the entire file or disk is searched, top down, and wildcards, whole words only, match case, Unicode, and “SaveAllPos” are not enabled. If you enable wildcards, the character chosen in WinHex (by default "?" or 0x3F, respectively) can be used as a placeholder for one byte.</param>
        /// <returns>The result is TRUE if the search could be completed and if no error occurred.</returns>
        [DllImport("whxapi")]
        private static extern bool WHX_Find(string lpData, string lpOptions);

        /// <summary>
        /// Replaces all occurrences of the data given by lpData1 with that given by lpData2. Can replace text strings as well as hexadecimal values. Supported options are “MatchCase”, “MatchWord”, “Down”, “Up”, “BlockOnly”, “Wildcards”, and “Unicode”. See WHX_Find for details.
        /// </summary>
        /// <returns>The result is TRUE if the replacement could be completed and if no error occurred.</returns>
        [DllImport("whxapi")]
        private static extern bool WHX_Replace(string lpData1, string lpData2, string lpOptions);

        /// <returns>Returns TRUE if the last executed WHX_Find or WHX_Replace function actually found the specified data.</returns>
        [DllImport("whxapi")]
        private static extern bool WHX_WasFound();

        /// <returns>Returns the number of occurrences found by WHX_Find (0 or 1, because WHX_Finds stops at the first occurrence, if any) or replaced by WHX_Replace (0, 1, or any other positive number). Available as of WinHex 11.4.</returns>
        [DllImport("whxapi")]
        private static extern int WHX_WasFoundEx();

        /// <summary>
        /// Converts the data in the active file from one format into another. Valid format strings are “ANSI”, “IBM”, “EBCDIC”, “Binary”, “HexASCII”, “IntelHex”, “MotorolaS”, “Base64”, “UUCode”, in combinations as known from the Convert menu command in WinHex.
        /// </summary>
        [DllImport("whxapi")]
        private static extern bool WHX_Convert(string lpSrcFormat, string lpDstFormat);

        /// <summary>
        /// Encrypts the active file, disk, or block selected therein with the PC1 algorithm and a hash of the key pointed to by lpKey (16 bytes long at max.) as the encryption key. You are responsable for zeroing the key pointed to by lpKey afterwards to maximize security.
        /// </summary>
        /// <param name="lpKey">hash of the key pointed to, 16 bytes long at max.</param>
        /// <param name="Algorithm">Algorithm must be 1</param>
        [DllImport("whxapi")]
        private static extern bool WHX_Encrypt(string lpKey, int Algorithm);

        /// <summary>
        /// Decrypts the active file, disk, or block selected therein with the PC1 algorithm and a hash of the key pointed to by lpKey (16 bytes long at max.) as the decryption key. You are responsable for zeroing the key pointed to by lpKey afterwards to maximize security.
        /// </summary>
        /// <param name="lpKey">hash of the key pointed to, 16 bytes long at max.</param>
        /// <param name="Algorithm">Algorithm must be 1</param>
        [DllImport("whxapi")]
        private static extern bool WHX_Decrypt(string lpKey, int Algorithm);

        /// <summary>
        /// Retrieves the name of the currently active file (including the path) or description of the currently active disk, respectively, in the buffer that lpObjName points to. Disk descriptions are returned in the language selected in WinHex. Available since WinHex 10.55. Description may vary with different WinHex versions.
        /// </summary>
        /// <param name="lpObjName">The provided buffer must be at least 256 bytes in size.</param>
        /// <returns></returns>
        [DllImport("whxapi")]
        private static extern bool WHX_GetCurObjName(StringBuilder lpObjName);

        /// <summary>
        /// The lowest bit in the integer parameter controls whether or not WinHex displays errors to the user by way of a message box. To retrieve the error message programmatically, use WHX_GetLastError. If you select not to let the user see the message box, WinHex will assume the user pressed “Cancel” or “No”, if he would have been given the option to choose between two buttons.
        /// The second-lowest bit controls whether or not WinHex displays the small progress window during lengthy operations, which often allow the user to abort by pressing Esc or closing the window or to pause by pressing the Pause key. Available since WinHex 11.1.
        /// </summary>
        /// <param name="Level">
        /// 0		Display message boxes and progress windows
        /// 1		Suppress message boxes only
        /// 2		Suppress progress windows only
        /// 3		Suppress message boxes and progress windows
        /// </param>
        [DllImport("whxapi")]
        private static extern bool WHX_SetFeedbackLevel(int Level);

        /// <summary>
        /// Retrieves the description of the last error that WinHex has displayed to the user (or would have displayed to the user, depending on the feedback level). The description is undefined if no error has occurred since the last use of WHX_Init.
        /// </summary>
        /// <param name="lpErrorMsg">The provided buffer must be at least 256 bytes in size.</param>
        [DllImport("whxapi")]
        private static extern bool WHX_GetLastError(StringBuilder lpErrorMsg);

        /// <summary>
        /// Specifies an error description to be retrieved by WHX_GetLastError, or clears it if lpErrorMsg points to an empty string or is null. Available since WinHex 10.55 SR-4.
        /// </summary>
        [DllImport("whxapi")]
        private static extern bool WHX_SetLastError(string lpErrorMsg);

        /// <summary>
        /// Retrieves the full path of the WinHex installation currently used by the API DLL in lpInstPath (e.g. “D:\Program Files\WinHex\winhex.exe”, up to 255 characters), the WinHex version in the lower WORD of the LONG value at the address specified by lpWHxVersion (e.g. 1100 means v11.00), and the WinHex service release number in the lower WORD of the LONG value at the address specified by lpWHxSubVersion (e.g. 1 means SR-1, 0 means no SR). All parameters are optional, so the pointers may be set to NULL. The pointer lpReserved is currently unused and must be set to NULL. Available since WinHex 11.0 SR-2.
        /// </summary>
        [DllImport("whxapi")]
        private static extern bool WHX_GetStatus(StringBuilder lpInstPath, out int lpWHXVersion, out int lpWHXSubVersion,
                           int lpReserved);
        #endregion

        [Flags]
        public enum OpenMode
        {
            ReadOnly = 0x1,
            InPlace = 0x2,
            Image = 0x4
        }

        [Flags]
        public enum FindOptions
        {
            Default = 0x0,
            MatchCase = 0x1,
            MatchWord = 0x2,
            Down = 0x4,
            Up = 0x8,
            BlockOnly = 0x10,
            SaveAllPos = 0x20,
            Wildcards = 0x40,
            Unicode = 0x80
        }
    }
}

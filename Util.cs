using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Forms;
using System.IO;
using System.Web.UI;
using System.Reflection;

namespace UtilLib
{
    // 文字列と数値のセット
    public class sTX_INT
    {
        public string m_txName;
        public int m_dtVal;

        public sTX_INT(string txName1, int dtVal1)
        {
            m_txName = txName1;
            m_dtVal = dtVal1;
        }

        public override string ToString()
        {
            return (m_txName);
        }
    }

    public enum eSER_BRT
    {
        _300,
        _600,
        _1200,
        _2400,
        _4800,
        _9600,
        _14400,
        _19200,
        _28800,
        _38400,
        _57600,
        _115200,
        _230400,
        _208333,
        _312500,
        LMT
    };

    // ハンドシェイク
    public enum eSER_HSK
    {
        _300,
        _600,
        _1200,
        _2400,
        _4800,
        _9600,
        _14400,
        _19200,
        _28800,
        _38400,
        _57600,
        _115200,
        _230400,
        _208333,
        _312500,
        LMT
    };

    public enum eUSB_BRT
    {
        _9600,
        _76800,
        LMT
    };

    public enum eREEL
    {
        L,
        C,
        R,
        LMT
    }

    public class Com
    {
        public const int RCV_CMD_LEN_MAX = 32;
        public const int SRI_RCV_DATA_SIZE = 256;
        public const int MMF_SIZE = 16384;
        public const int DFL_LOG_GAME = 10;
        public const int EASY_CHK_BUF_MAX = 1024;
        public const int EASY_CHK_DATA_MAX = 1024;

        static public string[] m_atxPortName;        // ポート
        static public sTX_INT[] m_asBaudRate;        // ボーレート
        static public sTX_INT[] m_asDataBits1;        // データビット
        static public sTX_INT[] m_asParity1;        // パリティビット
        static public sTX_INT[] m_asStopBits1;        // ストップビット
        static public sTX_INT[] m_asHandShake1;        // ハンドシェイク
        static public sTX_INT[] m_asUsbBaudRate;        // USBボーレート

        static Com()
        {
            //! 利用可能なシリアルポート名の配列を取得する.

            m_atxPortName = SerialPort.GetPortNames();

            m_asDataBits1 = new sTX_INT[] {
                new sTX_INT("5", 5),
                new sTX_INT("6", 6),
                new sTX_INT("7", 7),
                new sTX_INT("8", 8),
            };

            // ボーレート設定
            m_asBaudRate = new sTX_INT[(int)eSER_BRT.LMT] {
                new sTX_INT("300", 300),
                new sTX_INT("600", 600),
                new sTX_INT("1200", 1200),
                new sTX_INT("2400", 2400),
                new sTX_INT("4800", 4800),
                new sTX_INT("9600", 9600),
                new sTX_INT("14400", 14400),
                new sTX_INT("19200", 19200),
                new sTX_INT("28800", 28800),
                new sTX_INT("38400", 38400),
                new sTX_INT("57600", 57600),
                new sTX_INT("115200", 115200),
                new sTX_INT("230400", 230400),
                new sTX_INT("208333", 208333),
                new sTX_INT("312500", 312500),
            };

            // ハンドシェイク
            m_asHandShake1 = new sTX_INT[] {
                new sTX_INT("なし", (int)Handshake.None),
                new sTX_INT("XON/XOFF制御", (int)Handshake.XOnXOff),
                new sTX_INT("RTS/CTS制御", (int)Handshake.RequestToSend),
                new sTX_INT("XON/XOFF + RTS/CTS制御", (int)Handshake.RequestToSendXOnXOff),
            };

            // ストップビット
            m_asStopBits1 = new sTX_INT[] {
                new sTX_INT("なし", (int)StopBits.None),
                new sTX_INT("1", (int)StopBits.One),
                new sTX_INT("1.5", (int)StopBits.OnePointFive),
                new sTX_INT("2", (int)StopBits.Two),
            };

            // パリティ
            m_asParity1 = new sTX_INT[] {
                new sTX_INT("なし", (int)Parity.None),
                new sTX_INT("奇数", (int)Parity.Odd),
                new sTX_INT("偶数", (int)Parity.Even),
                new sTX_INT("マーク", (int)Parity.Mark),
                new sTX_INT("スペース", (int)Parity.Space),
            };

            // ボーレート設定
            m_asBaudRate = new sTX_INT[(int)eSER_BRT.LMT] {
                new sTX_INT("300", 300),
                new sTX_INT("600", 600),
                new sTX_INT("1200", 1200),
                new sTX_INT("2400", 2400),
                new sTX_INT("4800", 4800),
                new sTX_INT("9600", 9600),
                new sTX_INT("14400", 14400),
                new sTX_INT("19200", 19200),
                new sTX_INT("28800", 28800),
                new sTX_INT("38400", 38400),
                new sTX_INT("57600", 57600),
                new sTX_INT("115200", 115200),
                new sTX_INT("230400", 230400),
                new sTX_INT("208333", 208333),
                new sTX_INT("312500", 312500),
            };

            // ボーレート設定
            m_asUsbBaudRate = new sTX_INT[(int)eUSB_BRT.LMT] {
                new sTX_INT("9600", 9600),
                new sTX_INT("76800", 76800),
            };
        }

        public Com()
        {

        }

        // COM名からポート番号を取得
        public static int GetPortIndx(string txPort1)
        {
            int idPort1 = -1;

            int idPort2;
            for (idPort2 = 0; idPort2 < m_atxPortName.Length; idPort2++)
            {
                if (m_atxPortName[idPort2] == txPort1)
                {
                    idPort1 = idPort2;
                    break;
                }
            }

            return (idPort1);
        }

        public enum eBASE_KIND
        {
            _2,
            _8,
            _10,
            _16,
            LMT
        };

        public enum eFRM_CLOSE
        {
            NON,
            PRE,
            END,
            LMT
        };


        /// <summary>
        /// テキストから数値を取得します。
        /// </summary>
        /// <param name="tx1"></param>
        /// <returns></returns>
        static public int GetIntFromTxt(string tx1)
        {
            int iData1 = -1;
            int.TryParse(tx1, out iData1);
            return (iData1);
        }

        /// <summary>
        /// 指定の文字列にパスとして無効な文字が含まれていないか確認する
        /// </summary>
        /// <param name="txPath1"></param>
        /// <returns></returns>
        static public bool IsPathName(string txPath1)
        {
            bool bFlag1 = false;
            string txDirPath1 = System.IO.Path.GetDirectoryName(txPath1);
            char[] tbliChrDir1 = System.IO.Path.GetInvalidPathChars();

            string txFileName1 = System.IO.Path.GetFileName(txPath1);
            char[] tbliChrFile1 = System.IO.Path.GetInvalidFileNameChars();
            if (txDirPath1.IndexOfAny(tbliChrDir1) >= 0)
            {
                // フォルダパスに無効な文字がありました。
            }
            else if (txFileName1.IndexOfAny(tbliChrFile1) >= 0)
            {
                // ファイル名に無効な文字がありました。
            }
            else
            {
                bFlag1 = true;
            }
            return (bFlag1);
        }

        /// <summary>
        /// 指定パスが絶対パスかを確認
        /// </summary>
        /// <param name="txPath1"></param>
        /// <returns></returns>
        static public bool IsAbsPath(string txPath1)
        {
            bool bFlag1 = false;
            if (System.IO.Path.IsPathRooted(txPath1))
            {
                bFlag1 = true;
            }
            return (bFlag1);
        }

        //static public string GetExePath()
        //{
        //    return (Assembly.GetEntryAssembly().Location);
        //}

        // Exeパスを取得
        public static string GetExePath()
        {
            return (AppDomain.CurrentDomain.BaseDirectory);
        }

        // 指定のフォルダパス内にあるすべてのファイルリストを取得
        static public List<string> GetFileListFromPath(string txDirPath1)
        {
            return (Directory.GetFiles(txDirPath1, "*", System.IO.SearchOption.AllDirectories).ToList());
        }

        // 指定のファイルパスからファイル名を取得
        static public string GetFileName(string txPath1)
        {
            return (Path.GetFileName(txPath1));
        }

        // 指定のファイルパスからパスのみ取得
        static public string GetDriPath(string txPath1)
        {
            return (Path.GetDirectoryName(txPath1));
        }

        /// <summary>
        /// ダイアログよりフォルダパスを取得します。
        /// </summary>
        /// <param name="txDirPath1"></param>
        /// <param name="txTitle1"></param>
        /// <returns></returns>
        static public string GetDirPathFromDlg(string txDirPath1, string txTitle1)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            //string txInitDirPath1 = System.IO.Path.GetDirectoryName(txFileName1);

            ofd.FileName = "任意のファイル";
            ofd.InitialDirectory = txDirPath1;
            ofd.Filter = "すべてのファイル(*.*)|*.*";
            ofd.FilterIndex = 1;
            //if (txInitDirPath1 != "")
            //{
            //    ofd.InitialDirectory = txInitDirPath1;
            //}
            ofd.Title = txTitle1;
            ofd.RestoreDirectory = false;
            ofd.CheckFileExists = false;
            ofd.CheckPathExists = true;

            string txDirPath2 = "";
            //ダイアログを表示する
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string txPath1 = ofd.FileName;
                txDirPath2 = System.IO.Path.GetDirectoryName(txPath1);
            }

            return (txDirPath2);
        }

        /// <summary>
        /// ダイアログより読み込みファイルを取得します。
        /// </summary>
        /// <param name="txFileName1"></param>
        /// <param name="txTitle1"></param>
        /// <param name="txFilter1">Word Documents|*.doc|Office Files|*.doc;*.xls;*.ppt|All Files|*.*</param>
        /// <param name="bCheckFlag1">ファイルが存在するかをチェック</param>
        /// <returns></returns>
        static public string GetFilePathFromDlg(string txFileName1, string txTitle1, string txFilter1, bool bCheckFlag1 = false)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            string txFileName2 = "";
            string txInitDirPath1 = "";

            if (txFileName1 != "")
            {
                try
                {
                    txFileName2 = System.IO.Path.GetFileName(txFileName1);
                }
                catch
                {
                    txFileName2 = "";
                }
                txInitDirPath1 = System.IO.Path.GetDirectoryName(txFileName1);
            }

            ofd.FileName = txFileName2;
            ofd.Filter = txFilter1;
            ofd.FilterIndex = 1;
            if (txInitDirPath1 != "")
            {
                ofd.InitialDirectory = txInitDirPath1;
            }
            ofd.Title = txTitle1;
            ofd.RestoreDirectory = false;
            ofd.CheckFileExists = bCheckFlag1;
            ofd.CheckPathExists = true;

            string txPath1 = "";
            //ダイアログを表示する
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txPath1 = ofd.FileName;
            }

            return (txPath1);
        }

        /// <summary>
        /// ダイアログより読み込みファイルを取得します。
        /// </summary>
        /// <param name="txFileName1"></param>
        /// <param name="txTitle1"></param>
        /// <param name="txFilter1"></param>
        /// <param name="bCheckFlag1"></param>
        /// <param name="flMlt1"></param>
        /// <returns></returns>
        static public string[] GetFilePathFromDlg(string txFileName1, string txTitle1, string txFilter1, bool bCheckFlag1, bool flMlt1)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            string txFileName2 = "";
            string txInitDirPath1 = "";

            if (txFileName1 != "")
            {
                txFileName2 = System.IO.Path.GetFileName(txFileName1);
                txInitDirPath1 = System.IO.Path.GetDirectoryName(txFileName1);
            }

            ofd.FileName = txFileName2;
            ofd.Filter = txFilter1;
            ofd.FilterIndex = 1;
            if (txInitDirPath1 != "")
            {
                ofd.InitialDirectory = txInitDirPath1;
            }
            ofd.Title = txTitle1;
            ofd.RestoreDirectory = false;
            ofd.CheckFileExists = bCheckFlag1;
            ofd.CheckPathExists = true;
            ofd.Multiselect = flMlt1;

            string[] atxPath1 = new string[0];
            //ダイアログを表示する
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                atxPath1 = ofd.FileNames;
            }

            return (atxPath1);
        }

        /// <summary>
        /// ダイアログより保存先のファイルを取得します。
        /// </summary>
        /// <param name="txFileName1"></param>
        /// <param name="txTitle1"></param>
        /// <param name="txFilter1"></param>
        /// <param name="bCheckFlag1"></param>
        /// <returns></returns>
        static public string GetSaveFilePathFromDlg(string txFileName1, string txTitle1, string txFilter1, bool bCheckFlag1)
        {
            SaveFileDialog sfd1 = new SaveFileDialog();

            string txFileName2 = System.IO.Path.GetFileName(txFileName1);
            string txInitDirPath1 = System.IO.Path.GetDirectoryName(txFileName1);

            sfd1.FileName = txFileName2;
            sfd1.Filter = txFilter1;
            sfd1.FilterIndex = 1;
            if (txInitDirPath1 != "")
            {
                sfd1.InitialDirectory = txInitDirPath1;
            }
            sfd1.Title = txTitle1;
            sfd1.RestoreDirectory = false;
            sfd1.CheckFileExists = bCheckFlag1;
            sfd1.CheckPathExists = true;

            string txPath1 = "";
            //ダイアログを表示する
            if (sfd1.ShowDialog() == DialogResult.OK)
            {
                txPath1 = sfd1.FileName;
            }

            return (txPath1);
        }

        /// <summary>
        /// 文字列ファイルを指定のファイルに出力します。
        /// </summary>
        /// <param name="txFilePath1"></param>
        /// <param name="tbltxLine1"></param>
        static public void OutTblToFile(string txFilePath1, List<string> tbltxLine1)
        {
            try
            {
                //StreamWriter sw1 = new StreamWriter(txFilePath1, false, Encoding.GetEncoding("shift_jis"));

                StreamWriter sw1 = new StreamWriter(txFilePath1, false, Encoding.GetEncoding("sjis"));

                int iLine1;
                for (iLine1 = 0; iLine1 < tbltxLine1.Count; iLine1++)
                {
                    sw1.WriteLine(tbltxLine1[iLine1]);
                }
                //閉じる
                sw1.Close();


            }
            catch
            {

            }
        }

        /// <summary>
        /// ファイル名からファイルの内容の文字列配列を取得します。
        /// </summary>
        /// <param name="txFilePath1"></param>
        /// <returns></returns>
        static public List<string> GetTblFromFile(string txFilePath1)
        {
            List<string> tbltx1 = new List<string>();

            try
            {
                //StreamReader sr1 = new StreamReader(txFilePath1, System.Text.Encoding.GetEncoding("shift_jis"));
                StreamReader sr1 = new StreamReader(txFilePath1, GetFileEncoding(txFilePath1));
                //内容を一行ずつ読み込む
                while (sr1.Peek() > -1)
                {
                    tbltx1.Add(sr1.ReadLine());
                }
                //閉じる
                sr1.Close();

                return (tbltx1);
            }
            catch
            {
                return (tbltx1);
            }
        }

        static public System.Text.Encoding GetFileEncoding(string txFilePath1)
        {
            System.Text.Encoding sEnc1 = null;
            try
            {
                string str = null;

                // ファイルをbyte形で全て読み込み
                FileStream fs = new FileStream(txFilePath1, FileMode.Open);
                byte[] data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);
                fs.Close();

                // 文字エンコード推定（hnx8氏公開のDLL）
                var charCode =
                    Hnx8.ReadJEnc.ReadJEnc.JP.GetEncoding(data, data.Length, out str);

                if (charCode == null)
                {
                    sEnc1 = System.Text.Encoding.GetEncoding("Shift_JIS");
                }
                else
                {
                    sEnc1 = charCode.GetEncoding();
                }
            }
            catch
            {

            }

            return sEnc1;
        }


        /// <summary>
        /// 例外が発生した行番号を取得します。
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        static public int GetLineFromException(Exception e)
        {
            System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace(e, true);
            int lineNumber = stackTrace.GetFrame(0).GetFileLineNumber();
            return (lineNumber);
        }

        /// <summary>
        /// リスト配列から通常配列を取得します。
        /// </summary>
        /// <param name="adtByte1"></param>
        /// <returns></returns>
        static public byte[] GetByteAryFromList(List<byte> adtByte1)
        {
            byte[] adtVal1 = new byte[adtByte1.Count];
            int idByte1;
            for (idByte1 = 0; idByte1 < adtByte1.Count; idByte1++)
            {
                adtVal1[idByte1] = adtByte1[idByte1];
            }
            return (adtVal1);
        }

        /// <summary>
        /// 指定のバイト配列の指定のビットから指定のビット数分の値を取り出します。
        /// </summary>
        /// <param name="adtVal1"></param>
        /// <param name="idBit1"></param>
        /// <param name="ctBit1"></param>
        /// <returns></returns>
        static public long GetValFromBitArea(List<byte> adtVal1, int idBit1, int ctBit1)
        {
            byte[] adtVal2 = new byte[adtVal1.Count];
            int idByte1;
            for (idByte1 = 0; idByte1 < adtVal1.Count; idByte1++)
            {
                adtVal2[idByte1] = adtVal1[idByte1];
            }
            long dtVal1 = GetValFromBitArea(adtVal2, idBit1, ctBit1);
            return (dtVal1);
        }


        /// <summary>
        /// 指定のバイト配列の指定のビットから指定のビット数分の値を取り出します。
        /// </summary>
        /// <param name="adtVal1"></param>
        /// <param name="idBit1"></param>
        /// <param name="ctBit1"></param>
        /// <returns></returns>
        static public long GetValFromBitArea(byte[] adtVal1, int idBit1, int ctBit1)
        {
            long dtVal1 = 0;
            int idByte1;
            int idBit2;

            if (idBit1 + ctBit1 > adtVal1.Length * 8)
            {
                // 配列オーバー
                return (0);
            }

            if (ctBit1 == 0)
            {
                return (0);
            }

            idByte1 = idBit1 / 8;
            idBit2 = idBit1 % 8;

            try
            {
                while (true)
                {
                    if (idBit2 + ctBit1 <= 8)
                    {
                        dtVal1 = (dtVal1 << ctBit1) + (long)((adtVal1[idByte1] >> (8 - (idBit2 + ctBit1))) & ((1 << ctBit1) - 1));
                        break;
                    }
                    else
                    {
                        // バイトまたぎ
                        dtVal1 = (dtVal1 << (8 - idBit2)) + (long)(adtVal1[idByte1] & ((1 << (8 - idBit2)) - 1));
                    }
                    idByte1++;
                    ctBit1 -= 8 - idBit2;
                    idBit2 = 0;
                }
            }
            catch
            {
            }

            return (dtVal1);
        }

        /// <summary>
        /// 指定のバイト配列の指定のビットから指定のビット数分の値を取り出します。
        /// </summary>
        /// <param name="adtVal1"></param>
        /// <param name="idBit1"></param>
        /// <param name="ctBit1"></param>
        /// <param name="dtVal1"></param>
        static public void SetValToBitArea(byte[] adtVal1, int idBit1, int ctBit1, long dtVal1)
        {
            int idByte1;
            int idBit2;
            byte dtVal2;
            long dtMax1;

            if (idBit1 + ctBit1 > adtVal1.Length * 8)
            {
                // 配列オーバー
                return;
            }

            idByte1 = idBit1 / 8;
            idBit2 = idBit1 % 8;

            while (true)
            {
                byte ctMask1;
                int idBit3;
                int ctBit2;
                if (idBit2 + ctBit1 <= 8)
                {
                    idBit3 = 8 - (idBit2 + ctBit1);
                    ctMask1 = (byte)(((1 << ctBit1) - 1) << idBit3);
                    adtVal1[idByte1] &= (byte)~ctMask1;
                    adtVal1[idByte1] |= (byte)((dtVal1 << idBit3) & ctMask1);
                    break;
                }
                else
                {
                    // バイトまたぎ                 
                    ctBit2 = 8 - idBit2;
                    ctMask1 = (byte)((1 << ctBit2) - 1);
                    dtMax1 = (long)(1 << (ctBit1 - ctBit2));
                    dtVal2 = (byte)(dtVal1 / dtMax1);
                    dtVal1 = dtVal1 % dtMax1;
                    adtVal1[idByte1] &= (byte)~ctMask1;
                    adtVal1[idByte1] |= (byte)dtVal2;
                }
                idByte1++;
                ctBit1 -= 8 - idBit2;
                idBit2 = 0;
            }

        }


        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(
            HandleRef hWnd, int msg, IntPtr wParam, IntPtr lParam);
        private const int WM_SETREDRAW = 0x000B;

        /// <summary>
        /// コントロールの再描画を停止させる
        /// </summary>
        /// <param name="control">対象のコントロール</param>
        public static void BeginControlUpdate(System.Windows.Forms.Control control)
        {
            SendMessage(new HandleRef(control, control.Handle),
                WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero);
        }

        /// <summary>
        /// コントロールの再描画を再開させる
        /// </summary>
        /// <param name="control">対象のコントロール</param>
        public static void EndControlUpdate(System.Windows.Forms.Control control)
        {
            SendMessage(new HandleRef(control, control.Handle),
                WM_SETREDRAW, new IntPtr(1), IntPtr.Zero);
            //control.Invalidate();
            control.Refresh();
        }

        // タブあり文字列のタブを同等のスペースに変換
        static public string TabToSpace(string txSrc1, int ctChrTab1)
        {
            int idChr1 = 0;
            int ctChrSpc1 = 0;
            string txDst1 = "";

            int idChr2;
            for (idChr2 = 0; idChr2 < txSrc1.Length; idChr2++)
            {
                string txChr1 = txSrc1.Substring(idChr2, 1);
                if (txChr1 == "\t")
                {
                    // タブ文字
                    ctChrSpc1 = ctChrTab1 - (idChr1 % ctChrTab1);
                    txDst1 += new string(' ', ctChrSpc1);
                    idChr1 += ctChrSpc1;
                }
                else
                {
                    txDst1 += txChr1;
                    idChr1++;
                }
            }
            return (txDst1);
        }

        public static string MidB(string txSrc1, int idChrHead1, int ctChr1)
        {
            string txRes1 = "";
            Encoding ec1 = Encoding.GetEncoding("Shift_JIS");
            byte[] abtSrc1 = ec1.GetBytes(txSrc1);

            if (idChrHead1 >= abtSrc1.Length)
            {
                ctChr1 = 0;
            }
            else if (ctChr1 < 0)
            {
                ctChr1 = abtSrc1.Length - idChrHead1;
            }
            else if (idChrHead1 + ctChr1 > abtSrc1.Length)
            {
                ctChr1 = abtSrc1.Length - idChrHead1;
            }

            if (ctChr1 == 0)
            {
                txRes1 = "";
            }
            else
            {
                // 先頭文字が全角文字にまたがっているかどうかをチェック
                if (idChrHead1 > 0)
                {
                    string tx1 = ec1.GetString(abtSrc1, idChrHead1, ctChr1);
                    string tx2 = ec1.GetString(abtSrc1, idChrHead1 - 1, ctChr1 + 1);
                    if (tx1.Length == tx2.Length)
                    {
                        // またがっている
                        idChrHead1--;
                        ctChr1++;
                    }
                }

                // 後尾文字が全角文字にまたがっているかどうかをチェック
                if (idChrHead1 + ctChr1 < abtSrc1.Length)
                {
                    string tx1 = ec1.GetString(abtSrc1, idChrHead1, ctChr1);
                    string tx2 = ec1.GetString(abtSrc1, idChrHead1, ctChr1 + 1);
                    if (tx1.Length == tx2.Length)
                    {
                        // またがっている
                        ctChr1++;
                    }
                }
                txRes1 = ec1.GetString(abtSrc1, idChrHead1, ctChr1);
            }
            return (txRes1);
        }

        /// <summary>
        /// テキストから数値を取得します。
        /// </summary>
        /// <param name="txVal1"></param>
        /// <param name="idVal1"></param>
        /// <returns></returns>
        public static bool GetValFromTxt(string txVal1, out Int64 idVal1)
        {
            bool flOk1 = true;
            eBASE_KIND nbBaseKind = GetBaseFromTxt(txVal1);
            switch (nbBaseKind)
            {
            case eBASE_KIND._2:
                idVal1 = Convert.ToInt64(txVal1, 2);
                break;
            case eBASE_KIND._8:
                idVal1 = Convert.ToInt64(txVal1, 8);
                break;
            case eBASE_KIND._10:
                idVal1 = Convert.ToInt64(txVal1, 10);
                break;
            case eBASE_KIND._16:
                idVal1 = Convert.ToInt64(txVal1, 16);
                break;
            default:
                // 数値ではありません。
                idVal1 = 0;
                flOk1 = false;
                break;
            }
            return (flOk1);
        }

        /// <summary>
        /// テキストから数値を取得します。
        /// </summary>
        /// <param name="txVal1"></param>
        /// <param name="idVal1"></param>
        /// <returns></returns>
        public static bool GetValFromTxt(string txVal1, out UInt64 idVal1)
        {
            bool flOk1 = true;
            eBASE_KIND nbBaseKind = GetBaseFromTxt(txVal1);
            switch (nbBaseKind)
            {
            case eBASE_KIND._2:
                idVal1 = Convert.ToUInt64(txVal1, 2);
                break;
            case eBASE_KIND._8:
                idVal1 = Convert.ToUInt64(txVal1, 8);
                break;
            case eBASE_KIND._10:
                idVal1 = Convert.ToUInt64(txVal1, 10);
                break;
            case eBASE_KIND._16:
                idVal1 = Convert.ToUInt64(txVal1, 16);
                break;
            default:
                // 数値ではありません。
                idVal1 = 0;
                flOk1 = false;
                break;
            }
            return (flOk1);
        }

        /// <summary>
        /// テキストから数値を取得します。
        /// </summary>
        /// <param name="txVal1"></param>
        /// <returns></returns>
        public static Int64 GetValFromTxt(string txVal1)
        {
            Int64 dtVal1;
            if (!GetValFromTxt(txVal1, out dtVal1))
            {
                // エラー文字列
                dtVal1 = 0;
            }
            return (dtVal1);
        }

        /// <summary>
        /// テキストから数値を取得します。
        /// </summary>
        /// <param name="txVal1"></param>
        /// <param name="idVal1"></param>
        /// <returns></returns>
        public static bool GetValFromTxt(string txVal1, out int idVal1)
        {
            bool flOk1 = false;
            long idVal2;
            if (GetValFromTxt(txVal1, out idVal2))
            {
                flOk1 = true;
            }
            idVal1 = (int)idVal2;
            return (flOk1);
        }

        /// <summary>
        /// 文字列から基数を取得します。
        /// </summary>
        /// <param name="txVal1"></param>
        /// <returns></returns>
        public static eBASE_KIND GetBaseFromTxt(string txVal1)
        {
            eBASE_KIND nbBaseKind = eBASE_KIND.LMT;
            Match m1;
            if ((m1 = Regex.Match(txVal1, @"^\s*0x(?<Val>[0-9a-fA-F]+)\s*$")).Success)
            {
                // 16進数
                nbBaseKind = eBASE_KIND._16;
            }
            else if ((m1 = Regex.Match(txVal1, @"^\s*-?(?<Val>[0-9]+)\s*$")).Success)
            {
                // 10進数
                nbBaseKind = eBASE_KIND._10;
            }
            else if ((m1 = Regex.Match(txVal1, @"^\s*0o(?<Val>[0-8]+)\s*$")).Success)
            {
                // 8進数
                nbBaseKind = eBASE_KIND._8;
            }
            else if ((m1 = Regex.Match(txVal1, @"^\s*0b(?<Val>[0-1]+)\s*$")).Success)
            {
                nbBaseKind = eBASE_KIND._2;
            }
            return (nbBaseKind);
        }

        /// <summary>
        /// マウスポインタの位置からタブ番号を割り出します。
        /// </summary>
        /// <param name="sTbc1"></param>
        /// <param name="pt"></param>
        /// <returns></returns>
        public static int GetTabPageByTab(TabControl sTbc1, Point pt)
        {
            int idPage1;
            for (idPage1 = 0; idPage1 < sTbc1.TabPages.Count; idPage1++)
            {
                Rectangle rect1 = sTbc1.GetTabRect(idPage1);
                if (sTbc1.GetTabRect(idPage1).Contains(pt))
                {
                    break;
                }
            }

            if (idPage1 >= sTbc1.TabPages.Count)
            {
                idPage1 = -1;
            }

            return (idPage1);
        }

        /// <summary>
        /// 16進数文字列からバイト配列を取得します。
        /// </summary>
        /// <param name="txHex1"></param>
        /// <returns></returns>
        public static List<Byte> GetByteAryFromHex(string txHex1)
        {
            if (txHex1.Length % 2 == 1)
            {
                txHex1 = txHex1 + "0";
            }
            int ctByte2;
            ctByte2 = (txHex1.Length + 1) / 2;
            List<Byte> adtCmd1 = new List<byte>();
            int dtCmd1;

            int idByte1;
            for (idByte1 = 0; idByte1 < ctByte2; idByte1++)
            {
                string txHex2;
                txHex2 = txHex1.Substring(idByte1 * 2, 2);
                int.TryParse(txHex2, System.Globalization.NumberStyles.HexNumber, null, out dtCmd1);
                adtCmd1.Add((byte)dtCmd1);
            }
            return (adtCmd1);
        }

        /// <summary>
        /// カレントパスと相対パスから絶対パスを取得します。
        /// </summary>
        /// <param name="txBasePath1"></param>
        /// <param name="txRelPath1"></param>
        /// <returns></returns>
        public static string GetAbsPath(string txBasePath1, string txRelPath1)
        {
            string txAbsPath1 = "";

            if (File.Exists(txRelPath1))
            {
                // もともと絶対パス
                txAbsPath1 = txRelPath1;
            }
            else
            {
                txAbsPath1 = txBasePath1 + @"\" + txRelPath1;
            }

            return (txAbsPath1);
        }

        /// <summary>
        /// カレントパスと絶対パスから相対パスを取得します。
        /// </summary>
        /// <param name="txBathPath1"></param>
        /// <param name="txAbsPath1"></param>
        /// <returns></returns>
        public static string GetRelPath(string txBathPath1, string txAbsPath1)
        {
            string txRelPath1 = "";

            try
            {
                //u1を基準としたu2の相対Uriを取得する
                Uri u1 = new Uri(txBathPath1);
                Uri u2 = new Uri(txAbsPath1);

                //絶対Uriから相対Uriを取得する
                Uri relativeUri = u1.MakeRelativeUri(u2);
                //文字列に変換する
                string relativePath = relativeUri.ToString();
                relativePath = HttpUtility.UrlDecode(relativePath);
                //.NET Framework 1.1以前では次のようにする
                //string relativePath = u1.MakeRelative(u2);

                //"/"を"\"に変換する
                txRelPath1 = relativePath.Replace('/', '\\');
            }
            catch
            {
                txRelPath1 = txAbsPath1;
            }

            return (txRelPath1);
        }

        public static DirectoryInfo SafeCreateDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                return null;
            }
            return Directory.CreateDirectory(path);
        }

        public static Color GetColorFromTxt(string txColor1)
        {
            Color sColor1;
            sColor1 = Color.FromArgb((int)GetValFromTxt(txColor1));
            return (sColor1);
        }

        public static void CmvAry8_7(byte[] adtSrc1, out Byte[] adtDst1)
        {
            const int BIT_7 = 7;
            const int BIT_8 = 8;
            int idByte1;
            int idBit1;
            int idBit2;
            int idBit3;
            int idBit4;
            int idByte2;

            idByte2 = 0;
            idByte1 = 0;
            idBit1 = 0;

            int ctByteDst1 = (adtSrc1.Length * 8 - 1) / 7 + 1;
            adtDst1 = new byte[ctByteDst1];

            for (idByte2 = 0; idByte2 < adtDst1.Length; idByte2++)
            {
                idBit2 = idBit1 + BIT_7;
                if (idBit2 < BIT_8)
                {
                    // バイトまたがず
                    adtDst1[idByte2] = (byte)((adtSrc1[idByte1] >> (BIT_8 - idBit2)) & ((1 << BIT_7) - 1));
                    idBit1 = idBit2;
                }
                else
                {
                    // バイトまたぎ
                    // 1バイト目の抽出ビット数
                    idBit3 = BIT_8 - idBit1;
                    // 2バイト目の抽出ビット数
                    idBit4 = BIT_7 - idBit3;
                    if (idByte2 < adtDst1.Length - 1)
                    {
                        // 途中
                        adtDst1[idByte2] = (byte)(((adtSrc1[idByte1] & ((1 << idBit3) - 1)) << idBit4) |
                            ((adtSrc1[idByte1 + 1] >> (BIT_8 - idBit4)) & ((1 << idBit4) - 1)));
                    }
                    else
                    {
                        // 最終バイト
                        adtDst1[idByte2] = (byte)((adtSrc1[idByte1] & ((1 << idBit3) - 1)) << idBit4);
                    }
                    idByte1++;
                    idBit1 = idBit4;
                }
            }
        }

        // XMLデシリアライズ
        public static void Deserialize<T>(string txXmlFile1, ref T sData1, bool flErrDsp1 = false)
        {
            if (!File.Exists(txXmlFile1))
            {
                return;
            }

            try
            {
                XmlSerializer sXs1 = new XmlSerializer(typeof(T));
                FileStream sFs1 = new FileStream(txXmlFile1, FileMode.Open);

                sData1 = (T)sXs1.Deserialize(sFs1);

                sFs1.Close();
            }
            catch (Exception e1)
            {
                if (flErrDsp1)
                {
                    // エラー表示あり
                    MessageBox.Show("デシリアライズに失敗しました。\n" + e1.Message);
                }
            }

        }

        // XMLシリアライズ
        public static void Serialize<T>(string txXmlFile1, T sData1)
        {
            XmlSerializer sXs1;
            try
            {
                sXs1 = new XmlSerializer(typeof(T));
                FileStream sFs1 = new FileStream(txXmlFile1, FileMode.Create);

                sXs1.Serialize(sFs1, sData1);

                sFs1.Close();
            }
            catch (Exception e1)
            {
                MessageBox.Show("シリアライズに失敗しました。\n" + e1.Message);
            }
        }

        public static void CopyToData<T1, T2>(T1 source, T2 dest)
        // where T2 : struct//これがないとValueType vt = dest;ができない。
        {
            var destType = dest.GetType();
            var sourceType = source.GetType();
            //構造体の場合はValueTypeにいったん置き換えないと値が更新されない。
            //ValueType vt = dest;
            //構造体の場合は以下のdestをvtに置き換えが必要


            //お互いのフィールドとプロパティを列挙して名前が一致したものコピーする
            foreach (var destProperty in destType.GetProperties())
            {
                foreach (var sourceProprty in sourceType.GetProperties().Where(v => v.Name == destProperty.Name))
                {
                    destProperty.SetValue(dest, sourceProprty.GetValue(source));
                }
                foreach (var sourceField in sourceType.GetFields().Where(v => v.Name == destProperty.Name))
                {
                    destProperty.SetValue(dest, sourceField.GetValue(source));
                }
            }

            foreach (var destField in destType.GetFields())
            {
                foreach (var sourceProperty in sourceType.GetProperties().Where(v => v.Name == destField.Name))
                {
                    destField.SetValue(dest, sourceProperty.GetValue(source));
                }
                foreach (var sourceField in sourceType.GetFields().Where(v => v.Name == destField.Name))
                {
                    destField.SetValue(dest, sourceField.GetValue(source));
                }
            }

            //以下のはまた構造体のときに必要
            //構造体に戻す
            //dest = (T2)vt;
        }

        public static void InitData<T1>(T1 source)
        // where T2 : struct//これがないとValueType vt = dest;ができない。
        {
            var sourceType = source.GetType();
            //構造体の場合はValueTypeにいったん置き換えないと値が更新されない。
            //ValueType vt = dest;
            //構造体の場合は以下のdestをvtに置き換えが必要


            //お互いのフィールドとプロパティを列挙して名前が一致したものコピーする

            foreach (PropertyInfo srcProperty in sourceType.GetProperties())
            {
                DefaultValueAttribute sAttName2 = (DefaultValueAttribute)srcProperty.GetCustomAttribute(typeof(DefaultValueAttribute));
                if (sAttName2 == null)
                {
                    throw new Exception(srcProperty.Name + "に" + "「DefaultValueAttribute」" + "属性が設定されていません。");
                }
                else
                {
                    srcProperty.SetValue(source, sAttName2.Value);
                }

            }

            //foreach (var destField in destType.GetFields())
            //{
            //    foreach (var sourceProperty in sourceType.GetProperties().Where(v => v.Name == destField.Name))
            //    {
            //        destField.SetValue(dest, sourceProperty.GetValue(source));
            //    }
            //    foreach (var sourceField in sourceType.GetFields().Where(v => v.Name == destField.Name))
            //    {
            //        destField.SetValue(dest, sourceField.GetValue(source));
            //    }
            //}

            //以下のはまた構造体のときに必要
            //構造体に戻す
            //dest = (T2)vt;
        }

        public static string GetSitTxt(List<bool> aflData1)
        {
            string txSit1 = "";

            int idDataPre1 = -1;
            int idData1;
            for (idData1 = 0; idData1 <= aflData1.Count; idData1++)
            {
                string txSit2 = "";
                if (idDataPre1 >= 0)
                {

                }

                bool flData1 = false;
                if (idData1 < aflData1.Count)
                {
                    flData1 = aflData1[idData1];
                }

                if (flData1)
                {
                    // データがTrue
                    if (idDataPre1 < 0)
                    {
                        // Trueの開始位置を保存
                        idDataPre1 = idData1;
                    }
                }
                else
                {
                    if (idDataPre1 >= 0)
                    {
                        // Trueの終了
                        if (idData1 - idDataPre1 > 1)
                        {
                            // Trueが2連続以上なら-表記
                            txSit2 = idDataPre1.ToString() + "-" + (idData1 - 1).ToString();
                        }
                        else
                        {
                            txSit2 = idDataPre1.ToString();
                        }
                        idDataPre1 = -1;
                    }
                }

                if (txSit2 != "")
                {
                    if (txSit1 == "")
                    {
                        txSit1 = txSit2;
                    }
                    else
                    {
                        txSit1 += "," + txSit2;
                    }
                }
            }

            return (txSit1);
        }

        public static List<int> GetSitAry(string txSit1)
        {
            List<int> aidData1 = new List<int>();
            Match m1;
            string txSit2;

            try
            {
                while (txSit1 != "")
                {
                    if ((m1 = Regex.Match(txSit1, @"^(?<Sit>.*?),(?<Last>.*)$")).Success)
                    {
                        txSit2 = m1.Groups["Sit"].Value;
                        txSit1 = m1.Groups["Last"].Value;
                    }
                    else
                    {
                        txSit2 = txSit1;
                        txSit1 = "";
                    }

                    if ((m1 = Regex.Match(txSit2, @"^\s*(?<Sta>\d+)\s*-\s*(?<End>\d+)\s*$")).Success)
                    {
                        string txSta1 = m1.Groups["Sta"].Value;
                        string txEnd1 = m1.Groups["End"].Value;

                        int idDataSta1 = int.Parse(txSta1);
                        int idDataEnd1 = int.Parse(txEnd1);

                        int idData1;
                        for (idData1 = idDataSta1; idData1 <= idDataEnd1; idData1++)
                        {
                            aidData1.Add(idData1);
                        }
                    }
                    else
                    {
                        int idData1 = int.Parse(txSit2);
                        aidData1.Add(idData1);
                    }
                }
            }
            catch
            {
                aidData1 = null;
            }

            return (aidData1);
        }

        public static List<bool> GetSitBoolAry(string txSit1, int ctData1)
        {
            return (GetBoolAry(GetSitAry(txSit1), ctData1));
        }

        public static List<bool> GetBoolAry(List<int> aidData1, int ctData1)
        {

            if (aidData1 == null)
            {
                return (null);
            }

            List<bool> aflData1 = new List<bool>();
            int idData1;
            for (idData1 = 0; idData1 < ctData1; idData1++)
            {
                aflData1.Add(false);
            }

            for (idData1 = 0; idData1 < aidData1.Count; idData1++)
            {
                if (aidData1[idData1] < ctData1)
                {
                    aflData1[aidData1[idData1]] = true;
                }
            }

            return (aflData1);
        }

        // 指定の文字列のSBDMハッシュ値を返します。
        public static int GetHashSbdm(string txData1)
        {
            int dtHash1 = 0;

            byte[] adtData1 = System.Text.Encoding.ASCII.GetBytes(txData1);

            foreach(byte dtData1 in adtData1)
            {
                dtHash1 = GetHashSbdm(dtHash1, dtData1);
            }

            return (dtHash1);
        }

        // SBDMハッシュ値を指定の値で更新します。
        public static int GetHashSbdm(int dtHash1, int dtChr1)
        {
            return (((dtChr1) + (dtHash1 << 6) + (dtHash1 << 16) - (dtHash1)));
        }

    }

}

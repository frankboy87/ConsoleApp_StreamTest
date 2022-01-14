using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp_StreamTest
{
    class Program
    {

        static void Main(string[] args)
        {

            Console.WriteLine("--------------FileStream write---------------");
            WriteFileByFileStream("retutn -1,if there are no charactors to read中文");

            Console.WriteLine("--------------StreamWrite---------------");
            string strToWrite = "StreamReader defaults to UTF8-Encoding unless specified otherwise";
            WriteFileByStream(strToWrite, "testc.log");

            Console.WriteLine("--------------ReadFileByStreamReader with file path---------------");
            ReadFileByStreamReader("testc.log");
            Console.WriteLine("---------------Read file by stream reader with stream----------------");
            FileStream fileStream = File.Open("testc.log", FileMode.Open);
            ReadFileByStreamReader(fileStream);
            fileStream.Dispose();

            Console.ReadKey();
        }

        /// <summary>
        /// writefile by filestream,not recommand
        /// </summary>
        /// <param name="strtoWrite"></param>
        public static void WriteFileByFileStream(string strtoWrite)
        {
            Console.WriteLine(strtoWrite);
            using (FileStream TextFile = new FileStream("testc.log", FileMode.Open,FileAccess.ReadWrite))
            {
                
                //byte[] Info = { (byte)'h', (byte)'e', (byte)'l', (byte)'l', (byte)'o' };
                //将字符串内容以GB2312编码写入,936代表GB2312,这样写入的代码文本打开才能不乱码
                TextFile.Write(Encoding.GetEncoding(936).GetBytes(strtoWrite), 0, Encoding.GetEncoding(936).GetByteCount(strtoWrite));
                TextFile.Flush();
                //Content is more diffcult than streamreader
                byte[] bytes = new byte[6];
                TextFile.Read(bytes, 0,6);
                string re = Encoding.GetEncoding(936).GetString(bytes);
            }
        }
        /// <summary>
        /// writefile by stream ,recommand that it is better than filestreamreader
        /// </summary>
        /// <param name="strToWrite"></param>
        /// <param name="filepath"></param>
        public static void WriteFileByStream(string strToWrite,string filepath)
        {

            using (StreamWriter streamWriter = new StreamWriter(filepath,true, Encoding.GetEncoding(936)))//写入流的时候直接确认编码方式
            {
                streamWriter.WriteLine(strToWrite);
                streamWriter.Flush();//将缓存立即写入文件(baseStream),并立即清空缓存内容;如果不调用将在close()方法执行完成后才写入文件;也可以AutoFlush为true实现
                streamWriter.WriteLine("流文件读取默认为UTF8编码,除非具体说名");
                streamWriter.Flush();
            }
        }
        #region StreamReader is better than FileStreamReader,FileStream can only readbytes,but streamreader can readline.
        /// <summary>
        /// 流文件通过文件路径读取
        /// </summary>
        public static void ReadFileByStreamReader(string filepath)
        {
            //写的时候用GB2312,读的时候自然用同样的编码方式读取才不会乱码
            using (StreamReader stream = new StreamReader(filepath, Encoding.GetEncoding(936)))
            {
                while (stream.Peek() > -1)//return -1 if this are no charactors to be read
                {
                    Console.WriteLine(System.Text.Encoding.Default.GetString(new byte[] { (byte)stream.Peek() })); //decode all bytes into string and the print
                    Console.WriteLine(stream.ReadLine());

                    System.Threading.Thread.Sleep(500);

                }
            }

        }
        /// <summary>
        /// 流文件通过stream读取
        /// </summary>
        /// <param name="stream"></param>
        public static void ReadFileByStreamReader(Stream stream)
        {
            StreamReader streamReader = new StreamReader(stream, Encoding.GetEncoding(936));
            Console.WriteLine(streamReader.ReadToEnd());
            //read done,must dispose
            streamReader.Dispose();
        }
        #endregion
    }
}


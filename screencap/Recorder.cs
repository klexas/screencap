using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SharpAvi;
// Contains types used for writing like AviWriter
using SharpAvi.Output;
// Contains types related to encoding like Mpeg4VideoEncoderVcm
using SharpAvi.Codecs;

namespace screencap
{
    public class Recorder
    {
        public string Name { get; set; }
        public string Location{ get; set; }
        private DateTime DateCreated { get; set; }
        private int screenWidth { get; set; }
        private int screenHeight { get; set; }
        private AviWriter writer { get; set; }
        private int framerate { get; set; }
        private int quality { get; set; }
        private IAviVideoStream stream { get; set; }
        private List<Bitmap> _frameBuffer { get; set; }
        public Recorder BuildRecorder(string name, string location, int width, int height, decimal framerate, decimal quality)
        {
            // Early out. 
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(location))
                throw new ArgumentNullException("Arguments must be valid.");
            if (new DirectoryInfo(location) == null) 
                throw new DirectoryNotFoundException("Directory not found");
            // Set the date
            this.DateCreated = DateTime.Now;
            this.Name = name;
            this.Location = location;
            this.screenWidth = width;
            this.screenHeight = height;
            this.framerate = Int32.Parse(framerate.ToString());
            this.quality = Int32.Parse(quality.ToString());
            this.writer = new AviWriter(this.Location + "\\" +this.Name)
            {
                FramesPerSecond = this.framerate,
                EmitIndex1 = true,
            };
            var encoder = new MotionJpegVideoEncoderWpf(width, height, this.quality);
            this.stream = writer.AddEncodingVideoStream(encoder, width: width, height: height);
            return this;
        }

        /// <summary>
        ///  Start the recording once all params are ready 
        /// </summary>
        public void StartRecording()
        {
            // Clear the frame buffer. 
            // TODO: Implement a pause feature which will not clear 
            _frameBuffer = new List<Bitmap>();
        }

        public void AddToStream(Bitmap frame)
        {
            _frameBuffer.Add(frame);
        }

        public bool SaveRecording()
        {
            bool saveSucess = false;
            // instantiate AVI writer, use WMV3 codec
            try
            {
                var frameData = new byte[stream.Width * stream.Height * 4];
                for (int i = 0; i < _frameBuffer.Count; i++)
                {
                    // Need this in order to properly read bitmap
                    ReleaseBuffer(frameData, _frameBuffer[i]);
                    stream.WriteFrame(true, // is key frame? (many codecs use concept of key frames, for others - all frames are keys)
                                      frameData, // array with frame data
                                      0, // starting index in the array
                                      frameData.Length // length of the data
                    );
                }
                writer.Close();
                saveSucess = true;
            }
            catch (Exception)
            {
                saveSucess = false;
            }
            return saveSucess;
        }

        private void ReleaseBuffer(byte[] buffer, Bitmap bitmap)
        {
            // We need to mirror flip the byte array when not compressed..
            // Not sure why, only noticed from output.. 
            // Better to do this on the bitmap obj
            //bitmap.RotateFlip(RotateFlipType.Rotate180FlipX);
            using (bitmap)
            {
                var bits = bitmap.LockBits(new Rectangle(0, 0, screenWidth, screenHeight), ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);
                Marshal.Copy(bits.Scan0, buffer, 0, buffer.Length);
                bitmap.UnlockBits(bits);
            }
        }

        /// <summary>
        /// Stop the recording. 
        /// </summary>
        public void StopRecording()
        {
            // First try to finalise the recording. 
            if (!_finalizeRecording(this));
            return;
        }

        private static bool _finalizeRecording(Recorder recording)
        {
            bool _returnResult = false;
            return _returnResult;
        }
    }
}

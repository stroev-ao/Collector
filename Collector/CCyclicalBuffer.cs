using System.Collections.Generic;
using System.Drawing;

namespace Collector
{
    public class CCyclicalBuffer
    {
        int frame;
        List<Bitmap> buffer;

        public CCyclicalBuffer()
        {
            buffer = new List<Bitmap>();
        }

        ~CCyclicalBuffer()
        {
            buffer.Clear();
            buffer = null;
        }

        public void AddImage(Bitmap bmp)
        {
            buffer.Add(bmp);
        }

        public Bitmap GetFrame()
        {
            if (frame == buffer.Count)
                frame = 0;

            return new Bitmap(buffer[frame++]);
        }

        public void Clear()
        {
            frame = 0;
            buffer.Clear();
        }
    }
}

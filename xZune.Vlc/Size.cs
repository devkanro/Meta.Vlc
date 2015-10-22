using System;

namespace xZune.Vlc
{
    [Serializable]
    public struct Size
    {
        public Size(double width, double height)
        {
            Width = width;
            Height = height;
        }
        public double Width { get; set; }
        public double Height { get; set; }

        public static bool operator ==(Size size1, Size size2)
        {
            return size1.Height.Equals(size2.Height) && size1.Width.Equals(size2.Width);
        }

        public static bool operator !=(Size size1, Size size2)
        {
            return !(size1 == size2);
        }
        
        public bool Equals(Size size)
        {
            return size == this;
        }

        public override bool Equals(object obj)
        {
            if (obj is Size)
            {
                return (Size)obj == this;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return Width.GetHashCode();
        }
    }
}

//Project: xZune.Vlc (https://github.com/higankanshi/xZune.Vlc)
//Filename: Size.cs
//Version: 20151108

using System;

namespace xZune.Vlc
{
  [Serializable]
  public struct Size
  {
    public Size(double width, double height)
      : this() //needed for VS2013 to compile, else complains that "this" is used before all of its fields have been assigned
    {
      Width = width;
      Height = height;
    }

    #region --- Properties ---

    public double Width { get; set; }
    public double Height { get; set; }

    #endregion

    #region --- Operators ---

    public static bool operator ==(Size size1, Size size2)
    {
      return size1.Height.Equals(size2.Height) && size1.Width.Equals(size2.Width);
    }

    public static bool operator !=(Size size1, Size size2)
    {
      return !(size1 == size2);
    }

    #endregion

    #region --- Methods ---

    public bool Equals(Size size)
    {
      return (size == this);
    }

    public override bool Equals(object obj)
    {
      if (obj is Size)
        return (Size)obj == this;
      else
        return false;
    }

    public override int GetHashCode()
    {
      return Width.GetHashCode();
    }

    #endregion
  }
}

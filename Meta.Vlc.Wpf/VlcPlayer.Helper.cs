// Project: Meta.Vlc (https://github.com/higankanshi/Meta.Vlc)
// Filename: VlcPlayer.Helper.cs
// Version: 20160214

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Meta.Vlc.Wpf
{
    public partial class VlcPlayer
    {
        #region Property Helpers

        private Size GetScaleTransform()
        {
            if (_context == null) return new Size(1.0, 1.0);

            AspectRatio aspectRatio = AspectRatio.Default;

            Dispatcher.Invoke(new Action(() => { aspectRatio = AspectRatio; }));

            Size scale = new Size(_context.DisplayWidth/_context.Width, _context.DisplayHeight/_context.Height);

            switch (aspectRatio)
            {
                case AspectRatio.Default:
                    return scale;

                case AspectRatio._16_9:
                    return new Size(1.0*_context.DisplayHeight/9*16/_context.Width,
                        1.0*_context.DisplayHeight/_context.Height);

                case AspectRatio._4_3:
                    return new Size(1.0*_context.DisplayHeight/3*4/_context.Width,
                        1.0*_context.DisplayHeight/_context.Height);
            }
            return new Size(1.0, 1.0);
        }

        #endregion Property Helpers

        #region Coordinate Helpers

        private int GetVideoPositionX(double x)
        {
            if (_context == null)
            {
                return (int) x;
            }
            double width = _context.Width*ScaleTransform.ScaleX,
                height = _context.Height*ScaleTransform.ScaleY;
            var px = 0;
            double scale, scaleX, scaleY;
            switch (Stretch)
            {
                case Stretch.None:
                    switch (HorizontalContentAlignment)
                    {
                        case HorizontalAlignment.Left:
                            px = (int) x;
                            break;

                        case HorizontalAlignment.Center:
                            px = (int) (x - ((ActualWidth - width)/2));
                            break;

                        case HorizontalAlignment.Right:
                            px = (int) (x - (ActualWidth - width));
                            break;

                        case HorizontalAlignment.Stretch:
                            if (ActualWidth > width)
                            {
                                px = (int) (x - ((ActualWidth - width)/2));
                            }
                            else
                            {
                                px = (int) x;
                            }
                            break;
                    }
                    break;

                case Stretch.Fill:
                    switch (StretchDirection)
                    {
                        case StretchDirection.UpOnly:
                            if (ActualWidth > width)
                            {
                                px = (int) (x/ActualWidth*width);
                            }
                            break;

                        case StretchDirection.DownOnly:
                            if (ActualWidth < width)
                            {
                                px = (int) (x/ActualWidth*width);
                            }
                            break;

                        case StretchDirection.Both:
                            px = (int) (x/ActualWidth*width);
                            break;
                    }
                    break;

                case Stretch.Uniform:
                    scaleX = ActualWidth/width;
                    scaleY = ActualHeight/height;
                    scale = Math.Min(scaleX, scaleY);

                    switch (StretchDirection)
                    {
                        case StretchDirection.UpOnly:
                            if (scale > 1)
                            {
                                if (scaleX == scale)
                                {
                                    px = (int) (x/scale);
                                }
                                else
                                {
                                    switch (HorizontalContentAlignment)
                                    {
                                        case HorizontalAlignment.Left:
                                            px = (int) (x/scale);
                                            break;

                                        case HorizontalAlignment.Center:
                                            px = (int) ((x - ((ActualWidth - width*scale)/2))/scale);
                                            break;

                                        case HorizontalAlignment.Right:
                                            px = (int) ((x - (ActualWidth - width*scale))/scale);
                                            break;

                                        case HorizontalAlignment.Stretch:
                                            px = (int) ((x - ((ActualWidth - width*scale)/2))/scale);
                                            break;

                                        default:
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                switch (HorizontalContentAlignment)
                                {
                                    case HorizontalAlignment.Left:
                                        px = (int) x;
                                        break;

                                    case HorizontalAlignment.Center:
                                        px = (int) (x - ((ActualWidth - width)/2));
                                        break;

                                    case HorizontalAlignment.Right:
                                        px = (int) (x - (ActualWidth - width));
                                        break;

                                    case HorizontalAlignment.Stretch:
                                        if (ActualWidth > width)
                                        {
                                            px = (int) (x - ((ActualWidth - width)/2));
                                        }
                                        else
                                        {
                                            px = (int) x;
                                        }
                                        break;
                                }
                            }
                            break;

                        case StretchDirection.DownOnly:
                            if (scale < 1)
                            {
                                if (scaleX == scale)
                                {
                                    px = (int) (x/scale);
                                }
                                else
                                {
                                    switch (HorizontalContentAlignment)
                                    {
                                        case HorizontalAlignment.Left:
                                            px = (int) (x/scale);
                                            break;

                                        case HorizontalAlignment.Center:
                                            px = (int) ((x - ((ActualWidth - width*scale)/2))/scale);
                                            break;

                                        case HorizontalAlignment.Right:
                                            px = (int) ((x - (ActualWidth - width*scale))/scale);
                                            break;

                                        case HorizontalAlignment.Stretch:
                                            px = (int) ((x - ((ActualWidth - width*scale)/2))/scale);
                                            break;

                                        default:
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                switch (HorizontalContentAlignment)
                                {
                                    case HorizontalAlignment.Left:
                                        px = (int) x;
                                        break;

                                    case HorizontalAlignment.Center:
                                        px = (int) (x - ((ActualWidth - width)/2));
                                        break;

                                    case HorizontalAlignment.Right:
                                        px = (int) (x - (ActualWidth - width));
                                        break;

                                    case HorizontalAlignment.Stretch:
                                        if (ActualWidth > width)
                                        {
                                            px = (int) (x - ((ActualWidth - width)/2));
                                        }
                                        else
                                        {
                                            px = (int) x;
                                        }
                                        break;
                                }
                            }
                            break;

                        case StretchDirection.Both:
                            if (scaleX == scale)
                            {
                                px = (int) (x/scale);
                            }
                            else
                            {
                                switch (HorizontalContentAlignment)
                                {
                                    case HorizontalAlignment.Left:
                                        px = (int) (x/scale);
                                        break;

                                    case HorizontalAlignment.Center:
                                        px = (int) ((x - ((ActualWidth - width*scale)/2))/scale);
                                        break;

                                    case HorizontalAlignment.Right:
                                        px = (int) ((x - (ActualWidth - width*scale))/scale);
                                        break;

                                    case HorizontalAlignment.Stretch:
                                        px = (int) ((x - ((ActualWidth - width*scale)/2))/scale);
                                        break;
                                }
                            }
                            break;
                    }
                    break;

                case Stretch.UniformToFill:
                    scaleX = ActualWidth/width;
                    scaleY = ActualHeight/height;
                    scale = Math.Max(scaleX, scaleY);

                    switch (StretchDirection)
                    {
                        case StretchDirection.UpOnly:
                            if (scale > 1)
                            {
                                if (scaleX == scale)
                                {
                                    px = (int) (x/scale);
                                }
                                else
                                {
                                    switch (HorizontalContentAlignment)
                                    {
                                        case HorizontalAlignment.Left:
                                            px = (int) (x/scale);
                                            break;

                                        case HorizontalAlignment.Center:
                                            px = (int) ((x - ((ActualWidth - width*scale)/2))/scale);
                                            break;

                                        case HorizontalAlignment.Right:
                                            px = (int) ((x - (ActualWidth - width*scale))/scale);
                                            break;

                                        case HorizontalAlignment.Stretch:
                                            px = (int) (x/scale);
                                            break;

                                        default:
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                switch (HorizontalContentAlignment)
                                {
                                    case HorizontalAlignment.Left:
                                        px = (int) x;
                                        break;

                                    case HorizontalAlignment.Center:
                                        px = (int) (x - ((ActualWidth - width)/2));
                                        break;

                                    case HorizontalAlignment.Right:
                                        px = (int) (x - (ActualWidth - width));
                                        break;

                                    case HorizontalAlignment.Stretch:
                                        if (ActualWidth > width)
                                        {
                                            px = (int) (x - ((ActualWidth - width)/2));
                                        }
                                        else
                                        {
                                            px = (int) x;
                                        }
                                        break;
                                }
                            }
                            break;

                        case StretchDirection.DownOnly:
                            if (scale < 1)
                            {
                                if (scaleX == scale)
                                {
                                    px = (int) (x/scale);
                                }
                                else
                                {
                                    switch (HorizontalContentAlignment)
                                    {
                                        case HorizontalAlignment.Left:
                                            px = (int) (x/scale);
                                            break;

                                        case HorizontalAlignment.Center:
                                            px = (int) ((x - ((ActualWidth - width*scale)/2))/scale);
                                            break;

                                        case HorizontalAlignment.Right:
                                            px = (int) ((x - (ActualWidth - width*scale))/scale);
                                            break;

                                        case HorizontalAlignment.Stretch:
                                            px = (int) (x/scale);
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                switch (HorizontalContentAlignment)
                                {
                                    case HorizontalAlignment.Left:
                                        px = (int) x;
                                        break;

                                    case HorizontalAlignment.Center:
                                        px = (int) (x - ((ActualWidth - width)/2));
                                        break;

                                    case HorizontalAlignment.Right:
                                        px = (int) (x - (ActualWidth - width));
                                        break;

                                    case HorizontalAlignment.Stretch:
                                        if (ActualWidth > width)
                                        {
                                            px = (int) (x - ((ActualWidth - width)/2));
                                        }
                                        else
                                        {
                                            px = (int) x;
                                        }
                                        break;
                                }
                            }
                            break;

                        case StretchDirection.Both:
                            if (scaleX == scale)
                            {
                                px = (int) (x/scale);
                            }
                            else
                            {
                                switch (HorizontalContentAlignment)
                                {
                                    case HorizontalAlignment.Left:
                                        px = (int) (x/scale);
                                        break;

                                    case HorizontalAlignment.Center:
                                        px = (int) ((x - ((ActualWidth - width*scale)/2))/scale);
                                        break;

                                    case HorizontalAlignment.Right:
                                        px = (int) ((x - (ActualWidth - width*scale))/scale);
                                        break;

                                    case HorizontalAlignment.Stretch:
                                        px = (int) (x/scale);
                                        break;

                                    default:
                                        break;
                                }
                            }
                            break;
                    }
                    break;
            }
            return px;
        }

        private int GetVideoPositionY(double y)
        {
            double width = _context.Width*ScaleTransform.ScaleX,
                height = _context.Height*ScaleTransform.ScaleY;
            int py = 0;
            double scale, scaleX, scaleY;
            switch (Stretch)
            {
                case Stretch.None:
                    switch (VerticalContentAlignment)
                    {
                        case VerticalAlignment.Top:
                            py = (int) y;
                            break;

                        case VerticalAlignment.Center:
                            py = (int) (y - ((ActualHeight - height)/2));
                            break;

                        case VerticalAlignment.Bottom:
                            py = (int) (y - (ActualHeight - height));
                            break;

                        case VerticalAlignment.Stretch:
                            if (ActualHeight > height)
                            {
                                py = (int) (y - ((ActualHeight - height)/2));
                            }
                            else
                            {
                                py = (int) y;
                            }
                            break;
                    }
                    break;

                case Stretch.Fill:
                    switch (StretchDirection)
                    {
                        case StretchDirection.UpOnly:
                            if (ActualHeight > height)
                            {
                                py = (int) (y/ActualHeight*height);
                            }
                            break;

                        case StretchDirection.DownOnly:
                            if (ActualHeight < height)
                            {
                                py = (int) (y/ActualHeight*height);
                            }
                            break;

                        case StretchDirection.Both:
                            py = (int) (y/ActualHeight*height);
                            break;
                    }
                    break;

                case Stretch.Uniform:
                    scaleX = ActualWidth/width;
                    scaleY = ActualHeight/height;
                    scale = Math.Min(scaleX, scaleY);

                    switch (StretchDirection)
                    {
                        case StretchDirection.UpOnly:
                            if (scale > 1)
                            {
                                if (scaleY == scale)
                                {
                                    py = (int) (y/scale);
                                }
                                else
                                {
                                    switch (VerticalContentAlignment)
                                    {
                                        case VerticalAlignment.Top:
                                            py = (int) (y/scale);
                                            break;

                                        case VerticalAlignment.Center:
                                            py = (int) ((y - ((ActualHeight - height*scale)/2))/scale);
                                            break;

                                        case VerticalAlignment.Bottom:
                                            py = (int) ((y - (ActualHeight - height*scale))/scale);
                                            break;

                                        case VerticalAlignment.Stretch:
                                            py = (int) ((y - ((ActualHeight - height*scale)/2))/scale);
                                            break;

                                        default:
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                switch (VerticalContentAlignment)
                                {
                                    case VerticalAlignment.Top:
                                        py = (int) y;
                                        break;

                                    case VerticalAlignment.Center:
                                        py = (int) (y - ((ActualHeight - height)/2));
                                        break;

                                    case VerticalAlignment.Bottom:
                                        py = (int) (y - (ActualHeight - height));
                                        break;

                                    case VerticalAlignment.Stretch:
                                        if (ActualHeight > height)
                                        {
                                            py = (int) (y - ((ActualHeight - height)/2));
                                        }
                                        else
                                        {
                                            py = (int) y;
                                        }
                                        break;
                                }
                            }
                            break;

                        case StretchDirection.DownOnly:
                            if (scale < 1)
                            {
                                if (scaleY == scale)
                                {
                                    py = (int) (y/scale);
                                }
                                else
                                {
                                    switch (VerticalContentAlignment)
                                    {
                                        case VerticalAlignment.Top:
                                            py = (int) (y/scale);
                                            break;

                                        case VerticalAlignment.Center:
                                            py = (int) ((y - ((ActualHeight - height*scale)/2))/scale);
                                            break;

                                        case VerticalAlignment.Bottom:
                                            py = (int) ((y - (ActualHeight - height*scale))/scale);
                                            break;

                                        case VerticalAlignment.Stretch:
                                            py = (int) ((y - ((ActualHeight - height*scale)/2))/scale);
                                            break;

                                        default:
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                switch (VerticalContentAlignment)
                                {
                                    case VerticalAlignment.Top:
                                        py = (int) y;
                                        break;

                                    case VerticalAlignment.Center:
                                        py = (int) (y - ((ActualHeight - height)/2));
                                        break;

                                    case VerticalAlignment.Bottom:
                                        py = (int) (y - (ActualHeight - height));
                                        break;

                                    case VerticalAlignment.Stretch:
                                        if (ActualHeight > height)
                                        {
                                            py = (int) (y - ((ActualHeight - height)/2));
                                        }
                                        else
                                        {
                                            py = (int) y;
                                        }
                                        break;
                                }
                            }
                            break;

                        case StretchDirection.Both:
                            if (scaleY == scale)
                            {
                                py = (int) (y/scale);
                            }
                            else
                            {
                                switch (VerticalContentAlignment)
                                {
                                    case VerticalAlignment.Top:
                                        py = (int) (y/scale);
                                        break;

                                    case VerticalAlignment.Center:
                                        py = (int) ((y - ((ActualHeight - height*scale)/2))/scale);
                                        break;

                                    case VerticalAlignment.Bottom:
                                        py = (int) ((y - (ActualHeight - height*scale))/scale);
                                        break;

                                    case VerticalAlignment.Stretch:
                                        py = (int) ((y - ((ActualHeight - height*scale)/2))/scale);
                                        break;

                                    default:
                                        break;
                                }
                            }
                            break;
                    }
                    break;

                case Stretch.UniformToFill:
                    scaleX = ActualWidth/width;
                    scaleY = ActualHeight/height;
                    scale = Math.Max(scaleX, scaleY);

                    switch (StretchDirection)
                    {
                        case StretchDirection.UpOnly:
                            if (scale > 1)
                            {
                                if (scaleY == scale)
                                {
                                    py = (int) (y/scale);
                                }
                                else
                                {
                                    switch (VerticalContentAlignment)
                                    {
                                        case VerticalAlignment.Top:
                                            py = (int) (y/scale);
                                            break;

                                        case VerticalAlignment.Center:
                                            py = (int) ((y - ((ActualHeight - height*scale)/2))/scale);
                                            break;

                                        case VerticalAlignment.Bottom:
                                            py = (int) ((y - (ActualHeight - height*scale))/scale);
                                            break;

                                        case VerticalAlignment.Stretch:
                                            py = (int) (y/scale);
                                            break;

                                        default:
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                switch (VerticalContentAlignment)
                                {
                                    case VerticalAlignment.Top:
                                        py = (int) y;
                                        break;

                                    case VerticalAlignment.Center:
                                        py = (int) (y - ((ActualHeight - height)/2));
                                        break;

                                    case VerticalAlignment.Bottom:
                                        py = (int) (y - (ActualHeight - height));
                                        break;

                                    case VerticalAlignment.Stretch:
                                        if (ActualHeight > height)
                                        {
                                            py = (int) (y - ((ActualHeight - height)/2));
                                        }
                                        else
                                        {
                                            py = (int) y;
                                        }
                                        break;
                                }
                            }
                            break;

                        case StretchDirection.DownOnly:
                            if (scale < 1)
                            {
                                if (scaleY == scale)
                                {
                                    py = (int) (y/scale);
                                }
                                else
                                {
                                    switch (VerticalContentAlignment)
                                    {
                                        case VerticalAlignment.Top:
                                            py = (int) (y/scale);
                                            break;

                                        case VerticalAlignment.Center:
                                            py = (int) ((y - ((ActualHeight - height*scale)/2))/scale);
                                            break;

                                        case VerticalAlignment.Bottom:
                                            py = (int) ((y - (ActualHeight - height*scale))/scale);
                                            break;

                                        case VerticalAlignment.Stretch:
                                            py = (int) (y/scale);
                                            break;

                                        default:
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                switch (VerticalContentAlignment)
                                {
                                    case VerticalAlignment.Top:
                                        py = (int) y;
                                        break;

                                    case VerticalAlignment.Center:
                                        py = (int) (y - ((ActualHeight - height)/2));
                                        break;

                                    case VerticalAlignment.Bottom:
                                        py = (int) (y - (ActualHeight - height));
                                        break;

                                    case VerticalAlignment.Stretch:
                                        if (ActualHeight > height)
                                        {
                                            py = (int) (y - ((ActualHeight - height)/2));
                                        }
                                        else
                                        {
                                            py = (int) y;
                                        }
                                        break;
                                }
                            }
                            break;

                        case StretchDirection.Both:
                            if (scaleY == scale)
                            {
                                py = (int) (y/scale);
                            }
                            else
                            {
                                switch (VerticalContentAlignment)
                                {
                                    case VerticalAlignment.Top:
                                        py = (int) (y/scale);
                                        break;

                                    case VerticalAlignment.Center:
                                        py = (int) ((y - ((ActualHeight - height*scale)/2))/scale);
                                        break;

                                    case VerticalAlignment.Bottom:
                                        py = (int) ((y - (ActualHeight - height*scale))/scale);
                                        break;

                                    case VerticalAlignment.Stretch:
                                        py = (int) (y/scale);
                                        break;

                                    default:
                                        break;
                                }
                            }
                            break;
                    }
                    break;
            }
            return py;
        }

        #endregion Coordinate Helpers
    }
}
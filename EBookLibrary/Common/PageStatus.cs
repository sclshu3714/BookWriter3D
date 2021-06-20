using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBookLibrary.Common
{
    public enum CornerOrigin { TopLeft, TopRight, BottomLeft, BottomRight };
    public enum PageStatus { None, Dragging, DraggingWithoutCapture, DropAnimation, TurnAnimation }
    public enum BookDisplayMode { Normal, ZoomOnPage }
    public enum BookCurrentPage { LeftSheet, RightSheet }
}

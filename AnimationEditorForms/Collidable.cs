﻿using AnimationEditorForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimationEditorForms
{
    public interface Collidable
    {
        bool Intersects(Circle circle);
        bool Intersects(RectangleF rect);
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MazeGame.Engine;

namespace MazeGame.RenderObjects
{
    /// <summary>
    /// A render component which can draw borders based upon coordinates
    /// </summary>
    public class Border : RenderObject
    {
        private bool _needsRender;
        public readonly BorderSection[] BorderSections;

        /// <summary>
        /// A border render object which takes in an optional array of border sections
        /// </summary>
        /// <param name="borderSections"></param>
        public Border(IEnumerable<BorderSection> borderSections = null)
        {
            Position = Vector2.Zero;
            Enabled = true;
            ZIndex = -100; // draw right at the back so that all content will be on top
            _needsRender = true;
            
            if (borderSections != null) BorderSections = borderSections.ToArray();
        }

        /// <summary>
        /// Update method
        /// </summary>
        /// <param name="updateInfo"></param>
        public override void Update(UpdateInfo updateInfo)
        {
            if (!_needsRender && updateInfo.HasResized) _needsRender = true;
        }

        /// <summary>
        /// render method
        /// </summary>
        public override void Render()
        {
            // only render if needed
            if (!_needsRender) return;
            
            Debug.WriteLine("Starting border render");
            
            // start with a blank frame and re-calculate the size of the border based upon the size of the display
            string[] render = Scene.Display.PopulateFrame();
            Size = new Vector2(Scene.Display.Width, Scene.Display.Height);

            // render the basic border outline around the whole screen
            render[0] = $"{Character.TopLeft}{Utils.Repeat(Character.Horizontal, Size.X - 2)}{Character.TopRight}";
            render[Size.Y - 1] = $"{Character.BottomLeft}{Utils.Repeat(Character.Horizontal, Size.X - 2)}{Character.BottomRight}";
            for (var i = 0; i < Size.Y - 2; i++)
            {
                render[i + 1] = $"{Character.Vertical}{Utils.Repeat(Character.Empty, Size.X - 2)}{Character.Vertical}";
            }

            // border sections is optional so check for that
            if (BorderSections != null)
            {
                // get every single corner position
                var allCorners = new List<Vector2>();
                
                /*
                 * Border section line drawing
                 */
                
                // draw each line of the border section
                // NOTE: this could be improved upon as 2 boxes next to each other will draw intersecting lines
                //       e.g box1's left line overwrites box2's right line
                foreach (var borderSection in BorderSections)
                {
                    // top line
                    try
                    {
                        render[borderSection.Position.Y] = render[borderSection.Position.Y]
                            .Remove(borderSection.Position.X + 1, borderSection.Size.X - 2)
                            .Insert(borderSection.Position.X + 1, Utils.Repeat(Character.Horizontal, borderSection.Size.X - 2));
                    }
                    catch (IndexOutOfRangeException e) {
                        Debug.WriteLine("Screen resized too small");
                        return;
                    }

                    // bottom line
                    try
                    {
                        render[borderSection.Position.Y + borderSection.Size.Y - 1] =
                            render[borderSection.Position.Y + borderSection.Size.Y - 1]
                                .Remove(borderSection.Position.X + 1, borderSection.Size.X - 2)
                                .Insert(borderSection.Position.X + 1,
                                    Utils.Repeat(Character.Horizontal, borderSection.Size.X - 2));
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        Debug.WriteLine("Screen resized too small");
                        return;
                    }

                    // left line
                    try
                    {
                        for (var i = 0; i < borderSection.Size.Y - 2; i++)
                        {
                            char[] line = render[borderSection.Position.Y + i + 1].ToCharArray();
                            line[borderSection.Position.X] = Character.Vertical;
                            render[borderSection.Position.Y + i + 1] = string.Join("", line);
                        }
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        Debug.WriteLine("Screen resized too small");
                        return;
                    }

                    // right line
                    try
                    {
                        for (var i = 0; i < borderSection.Size.Y - 2; i++)
                        {
                            char[] line = render[borderSection.Position.Y + i + 1].ToCharArray();
                            line[borderSection.Position.X + borderSection.Size.X - 1] = Character.Vertical;
                            render[borderSection.Position.Y + i + 1] = string.Join("", line);
                        }
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        Debug.WriteLine("Screen resized too small");
                        return;
                    }

                    // also calculate the 4 corners of every single border position
                    allCorners.Add(borderSection.Position);
                    allCorners.Add(borderSection.Position + new Vector2(borderSection.Size.X - 1, 0));
                    allCorners.Add(borderSection.Position + new Vector2(0, borderSection.Size.Y - 1));
                    allCorners.Add(borderSection.Position + borderSection.Size - Vector2.One);
                }

                // de duplicate the list of corners
                List<Vector2> deDupedCorners = allCorners.Distinct().ToList();

                // draw each corner
                foreach (var corner in deDupedCorners)
                {
                    /*
                     * figure out what character to draw
                     */

                    // NOTE: display a block by default so we can easily see that something has gone wrong
                    char borderCharacter = Character.SolidBlock;
                    
                    // skip the outer corners as they are already drawn by the initial border
                    if (corner.Equals(new Vector2(0, 0)) || corner.Equals(new Vector2(Scene.Display.Width - 1, 0))) continue;

                    // check if they are edge pieces
                    if (corner.Y == 0) borderCharacter = Character.TopCentre;
                    else if (corner.X == 0) borderCharacter = Character.MiddleLeft;
                    else if (corner.Y == Scene.Display.Height - 1) borderCharacter = Character.BottomCentre;
                    else if (corner.X == Scene.Display.Width - 1) borderCharacter = Character.MiddleRight;

                    // only start position checks if needed
                    if (borderCharacter == Character.SolidBlock)
                    {
                        // get the surrounding corners
                        bool isCornerToLeft = deDupedCorners.Count(c => (c.Y == corner.Y) && (c.X < corner.X)) >= 1;
                        bool isCornerToRight = deDupedCorners.Count(c => (c.Y == corner.Y) && (c.X > corner.X)) >= 1;
                        bool isCornerAbove = deDupedCorners.Count(c => (c.X == corner.X) && (c.Y < corner.Y)) >= 1;
                        bool isCornerBelow = deDupedCorners.Count(c => (c.X == corner.X) && (c.Y > corner.Y)) >= 1;
                        
                        Debug.WriteLine($"L: {isCornerToLeft} R: {isCornerToRight} U: {isCornerAbove} B: {isCornerBelow}");
                        
                        // get the right character based upon the position of closest corners
                        if (isCornerToRight && isCornerBelow && !isCornerAbove && !isCornerToLeft) borderCharacter = Character.TopLeft;
                        else if (isCornerToLeft && isCornerToRight && isCornerBelow && !isCornerAbove) borderCharacter = Character.TopCentre;
                        else if (isCornerToLeft && isCornerBelow && !isCornerToRight &&!isCornerAbove) borderCharacter = Character.TopRight;
                        else if (isCornerAbove && isCornerToRight && isCornerBelow && !isCornerToLeft) borderCharacter = Character.MiddleLeft;
                        else if (isCornerAbove && isCornerBelow && isCornerToLeft && isCornerToRight) borderCharacter = Character.Centre;
                        else if (isCornerAbove && isCornerToLeft && isCornerBelow && !isCornerToRight) borderCharacter = Character.MiddleRight;
                        else if (isCornerAbove && isCornerToRight && !isCornerBelow && !isCornerToLeft) borderCharacter = Character.BottomLeft;
                        else if (isCornerAbove && isCornerToLeft && isCornerToRight && !isCornerBelow) borderCharacter = Character.BottomCentre;
                        else if (isCornerAbove && isCornerToLeft && !isCornerBelow && !isCornerToRight) borderCharacter = Character.BottomRight;
                    }

                    // draw the corner character
                    try
                    {
                        char[] line = render[corner.Y].ToCharArray();
                        line[corner.X] = borderCharacter;
                        render[corner.Y] = string.Join("", line);
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        Debug.WriteLine("Screen resized too small");
                        return;
                    }
                }
            }

            _needsRender = false;
            Content = render;
        }
    }
}

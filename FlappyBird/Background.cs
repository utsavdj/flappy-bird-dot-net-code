using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FlappyBird
{
  class Background : Sprite
  {
    private const int SPEED = 1;
    private const int POSITION_X = 0;
    private const float SCREEN_HEIGHT_PERCENTAGE_FOR_POSITION_Y = 0.25f;
    private const float SCREEN_HEIGHT_PERCENTAGE_FOR_HEIGHT = 0.65f;

    private int _screenWidth;

    public Background(int screenWidth, int screenHeight) : 
      base(POSITION_X, (int)(screenHeight * SCREEN_HEIGHT_PERCENTAGE_FOR_POSITION_Y), 
        2 * screenWidth, (int)(screenHeight * SCREEN_HEIGHT_PERCENTAGE_FOR_HEIGHT))
    {
      _screenWidth = screenWidth;
    }

    public override void Display(Graphics graphics)
    {
      graphics.DrawImage(Properties.Resources.background, X, Y, Width, Height);
    }

    public override void Move()
    {
      if (X <= -_screenWidth)
      {
        X = 0;
      }
      X -= SPEED;
    }

  }
}

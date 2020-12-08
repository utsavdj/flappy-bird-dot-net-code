using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FlappyBird
{
  public abstract class Sprite
  {
    private int _x;
    private int _y;
    private int _width;
    private int _height;

    public Sprite(int x, int y, int width, int height)
    {
      _x = x;
      _y = y;
      _width = width;
      _height = height;
    }

    public abstract void Display(Graphics graphics);

    public abstract void Move();

    public int X
    {
      get
      {
        return _x;
      }

      set
      {
        _x = value;
      }
    }

    public int Y
    {
      get
      {
        return _y;
      }

      set
      {
        _y = value;
      }
    }

    public int Width
    {
      get
      {
        return _width;
      }
    }

    public int Height
    {
      get
      {
        return _height;
      }
    }

    public Rectangle BoundingBox
    {
      get
      {
        return new Rectangle(_x, _y, _width, _height);
      }
    }

    public bool Collided(Sprite other)
    {
      Rectangle boundingBoxOne = BoundingBox;
      Rectangle boundingBoxTwo = other.BoundingBox;
      return boundingBoxOne.IntersectsWith(boundingBoxTwo);
    }

    public int GenerateRandomNumber(int min, int max)
    {
      Random random = new Random();
      return random.Next(min, max);
    }

  }
}

using System;
using System.Collections.Generic;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace SnakeMono
{
    public class Snake
    {
        private Vector2 _head;
        public LinkedList<Vector2> Body { get; }
        private Direction _moveDirection;
        private Direction _lastMoveDirection;

        public Snake()
        {
            Body = new LinkedList<Vector2>();
            _head = new Vector2(Constants.StartingX, Constants.StartingY);
            _moveDirection = Direction.Nil;
            _lastMoveDirection = Direction.Nil;
            Body.AddFirst(_head);
        }

        private void MoveY(float value)
        {
            var temp = Body.First.Value;
            temp.X = _head.X;
            temp.Y = _head.Y + value;
            Body.RemoveFirst();
            Body.AddLast(temp);
            _head = temp;
        }

        private void MoveX(float value)
        {
            var temp = Body.First.Value;
            temp.X = _head.X + value;
            temp.Y = _head.Y;
            Body.RemoveFirst();
            Body.AddLast(temp);
            _head = temp;
        }

        private void MoveLeft()
        {
            MoveX(-Constants.FieldWidth);
            _lastMoveDirection = Direction.Left;
        }

        private void MoveRight()
        {
            MoveX(Constants.FieldWidth);
            _lastMoveDirection = Direction.Right;
        }

        private void MoveUp()
        {
            MoveY(-Constants.FieldHeight);
            _lastMoveDirection = Direction.Up;
        }

        private void MoveDown()
        {
            MoveY(Constants.FieldHeight);
            _lastMoveDirection = Direction.Down;
        }

        public void Move()
        {
            switch (_moveDirection)
            {
                case Direction.Left:
                    MoveLeft();
                    break;
                case Direction.Right:
                    MoveRight();
                    break;
                case Direction.Down:
                    MoveDown();
                    break;
                case Direction.Up:
                    MoveUp();
                    break;
                case Direction.Nil:
                    break;
                default:
                    break;
            }
        }

        public void SetDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    if (_lastMoveDirection != Direction.Right) _moveDirection = Direction.Left;
                    break;
                case Direction.Right:
                    if (_lastMoveDirection != Direction.Left) _moveDirection = Direction.Right;
                    break;
                case Direction.Up:
                    if (_lastMoveDirection != Direction.Down) this._moveDirection = Direction.Up;
                    break;
                case Direction.Down:
                    if (_lastMoveDirection != Direction.Up) this._moveDirection = Direction.Down;
                    break;
                case Direction.Nil:
                    throw new ArgumentException("Impossible!");
                default:
                    break;
            }
        }

        private int DirectionXOffset(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    return -Constants.FieldWidth;
                case Direction.Right:
                    return Constants.FieldWidth;
                default:
                    return 0;
            }
        }

        private static int DirectionYOffset(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return -Constants.FieldHeight;
                case Direction.Down:
                    return Constants.FieldHeight;
                default:
                    return 0;
            }
        }

        public bool WillEatHimself()
        {
            var next = NextPosition();
            foreach (var element in Body)
            {
                if (element.X == next.X && element.Y == next.Y && element != Body.First.Value)
                {
                    return true;
                }
            }

            return false;
        }
        

        public void Grow()
        {
            if (_moveDirection == Direction.Nil) return;
            _head = new Vector2(_head.X + DirectionXOffset(_moveDirection),
                _head.Y + DirectionYOffset(_moveDirection));
            Body.AddLast(_head);
        }

        public Vector2 NextPosition()
        {
            return new Vector2(_head.X + DirectionXOffset(_moveDirection),
                _head.Y + DirectionYOffset(_moveDirection));
        }

        public enum Direction
        {
            Up,
            Down,
            Right,
            Left,
            Nil
        };
    }
}
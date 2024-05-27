using AStarNavigator;
using AStarNavigator.Algorithms;
using Pacman_console.Models;

namespace Pacman_console
{
    public class Level
    {
        private readonly static string _field1 = 
                "███████████████████████████████\n" +
                "█..............█..............█\n" +
                "█♦████.███████.█.███████.████♦█\n" +
                "█.████.███████.█.███████.████.█\n" +
                "█.............................█\n" +
                "█.████.█.█████████████.█.████.█\n" +
                "█......█.......█.......█......█\n" +
                "██████.███████ █ ███████.██████\n" +
                "     █.█               █.█     \n" +
                "██████.█ ██████ ██████ █.██████\n" +
                "█╚    .█ ██         ██ █.    ╗█\n" +
                "██████.█ █████████████ █.██████\n" +
                "     █.█               █.█     \n" +
                "██████.█ █████████████ █.██████\n" +
                "█..............█..............█\n" +
                "█.████.███████.█.███████.████.█\n" +
                "█♦..██......... .........██..♦█\n" +
                "███.██.█.█████████████.█.██.███\n" +
                "█......█.......█.......█......█\n" +
                "█.████████████.█.████████████.█\n" +
                "█.............................█\n" +
                "███████████████████████████████\n";
        private readonly static string _field2 =
                "███████████████████████████████\n" +
                "█......███.....█.....███......█\n" +
                "█♦████.███.███.█.███.███.████♦█\n" +
                "█.████.███.███.█.███.███.████.█\n" +
                "█.............................█\n" +
                "█.████.█.█████████████.█.████.█\n" +
                "█......█.......█.......█......█\n" +
                "█████.████████ █ ████████.█████\n" +
                "    █..█               █..█    \n" +
                "██████.█ ██████ ██████ █.██████\n" +
                " ╚    .█ ██         ██ █.    ╗ \n" +
                "██████.█ █████████████ █.██████\n" +
                "    █..█               █..█    \n" +
                "█████.██ █████████████ ██.█████\n" +
                "█..............█..............█\n" +
                "█.████.███████.█.███████.████.█\n" +
                "█♦..█.......... ..........█..♦█\n" +
                "███.█.██.█████████████.██.█.███\n" +
                "█.....█........█........█.....█\n" +
                "█.███.█.██████.█.██████.█.███.█\n" +
                "█.....█.................█.....█\n" +
                "███████████████████████████████\n";

        private char[] _fieldArray = _field1.ToCharArray();

        private Player _player;
        private IList<Ghost>? _ghosts;

        public int Score = 0;

        public Level()
        {
             _player = new Player("Dave", 15, 16, 'O');
                
             _ghosts = new List<Ghost>()
             {
                 new("Sebastiaan", 15, 8, 'G'),
                 new("Vincent", 15, 10, 'G'),
                 new("Jelle", 13, 10, 'G'),
                 new("Hans", 17, 10, 'G'),
             };

            Draw();
        }

        public void Move(int directionX, int directionY)
        {
            var validPlayerDirection = !TileHasCollision(
                _player.Xposition + directionX,
                _player.Yposition + directionY
            );

            if (!validPlayerDirection)
            {
                return;
            }

            _player.Move(directionX, directionY);

            ComputePlayerTile();

            _fieldArray[GetPosition(_player.Xposition, _player.Yposition)] = ' ';

            MoveGhosts();

            TileCounter();

            Draw();
        }

        private void ComputePlayerTile()
        {
            char currenTile = GetTile(_player.Xposition, _player.Yposition);
            if (currenTile == '.')
            {
                Score += 10;
            }
            else if (currenTile == '♦')
            {
                Score += 50;
                _player.Energize();
            }
            else if (currenTile == '╗')
            {
                _player.Move(-27, 0);
            }
            else if (currenTile == '╚')
            {
                _player.Move(27, 0);
            }
        }

        private void MoveGhosts()
        {
            var navigator = new TileNavigator(
                new TileBlockedProvider(this),
                new DiagonalNeighborProvider(),
                new PythagorasAlgorithm(),
                new ManhattanHeuristicAlgorithm()
            );

            foreach (Ghost ghost in _ghosts)
            {
                DeathCheck(ghost);

                (int directionX, int directionY) direction;
                
                var algorithm = NumberGenerator.Generate(1, 3);
                if (algorithm == 1)
                {
                    direction = GetDirectionRandomly(ghost);
                }
                else
                {
                    direction = GetDirectionWithAStar(navigator, ghost);
                }


                ghost.Move(direction.directionX, direction.directionY);

                DeathCheck(ghost);
            }
        }

        private (int directionX, int directionY) GetDirectionRandomly(Ghost ghost)
        {
            bool validGhostDirection;

            (int directionX, int directionY) direction;

            do
            {
                direction = NumberGenerator.Generate(1, 5) switch
                {
                    1 => (0, 1),
                    2 => (1, 0),
                    3 => (0, -1),
                    4 => (-1, 0),
                    _ => throw new NotImplementedException()
                };

                validGhostDirection = !TileHasCollision(
                    ghost.Xposition + direction.directionX,
                    ghost.Yposition + direction.directionY
                );
            } while (!validGhostDirection);

            return direction;
        }

        private (int directionX, int directionY) GetDirectionWithAStar(TileNavigator navigator, Ghost ghost)
        {
            var from = new Tile(ghost.Xposition, ghost.Yposition);
            Tile to;
            if (_player.IsEnergized())
            {
                to = new Tile(15, 8);
            }
            else
            {
                to = new Tile(_player.Xposition, _player.Yposition);
            }

            var result = navigator.Navigate(from, to);

            if (result.Count() == 0)
            {
                return GetDirectionRandomly(ghost);
            }

            (int directionX, int directionY) direction = ((int)result.First().X - ghost.Xposition, (int)result.First().Y - ghost.Yposition);

            return direction;
        }

        private void DeathCheck(Ghost ghost)
        {
            if (_player.Xposition == ghost.Xposition
                && _player.Yposition == ghost.Yposition)
            {
                if (_player.IsEnergized())
                {
                    Score += 500;
                    ghost.Die();
                }
                else
                {
                    _player.Die();
                }
            }
        }

        private int GetPosition(int xPos, int yPos)
        {
            //calculates next position on array (-1 adjustment on the field for lives+score, 32 for row length)
            return xPos + (yPos * 32);
        }

        public bool TileHasCollision(int x, int y)
        {
            return GetTile(x, y) switch
            {
                '█' => true,
                '.' => false,
                '♦' => false,
                '╚' => false,
                '╗' => false,
                _ => false,
            };
        }

        private char GetTile(int x, int y)
        {
            return _fieldArray[GetPosition(x, y)];
        }

        private void TileCounter()
        {
            if (!_fieldArray.Contains('.') && !_fieldArray.Contains('♦'))
            {
                NextLevel();
            }
        }

        public void NextLevel()
        {
            if (NumberGenerator.Generate(1, 3) == 1)
            {
                _fieldArray = _field1.ToCharArray();
            }
            else
            {
                _fieldArray = _field2.ToCharArray();
            }

            _player.Xposition = 15;
            _player.Yposition = 16;

            _ghosts = new List<Ghost>()
            {
                new("Sebastiaan", 15, 8, 'G'),
                new("Vincent", 15, 10, 'G'),
                new("Jelle", 13, 10, 'G'),
                new("Hans", 17, 10, 'G'),
            };

        }

        public void Draw()
        {
            Console.Clear();

            Console.WriteLine($"Lives: {_player.Lives}        Score: {Score}");

            Console.Write(_fieldArray);

            _player.Draw();

            foreach (Ghost ghost in _ghosts)
            {
                ghost.Draw();
            }

            Console.SetCursorPosition(32, 1);
        }
    }
}
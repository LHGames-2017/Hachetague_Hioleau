namespace StarterProject.Web.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    [Route("/")]
    public class GameController : Controller
    {
        AIHelper player = new AIHelper();
        enum SubMoveAction
        {
            Default,
            MoveToRessource,
            ReturnToHouse,
            MoveToPlayer
        } //sub move action
        enum SubCollectAction
        {
            Default,
            CollectRessource,
            DropRessourceToHouse
        } //sub collect action
        enum SubUpgradeAction
        {
            UpgradeMaxHp,
            UpgradeDmg,
            UpgradeDef,
            UpgradeCap,
            Default
        }



        [HttpPost]
        public string Index([FromForm]string map)
        {
            GameInfo gameInfo = JsonConvert.DeserializeObject<GameInfo>(map);
            var carte = AIHelper.DeserializeMap(gameInfo.CustomSerializedMap);
            string action = string.Empty;
            ActionTypes PlayerState;
            SubMoveAction PlayerSubMoveState;
            ValidateFuturAction();

            // INSERT AI CODE HERE.
            
            SubCollectAction PlayerSubCollectState = SubCollectAction.Default;
            //SubUpgradeAction PlayerSubUpgradeState = SubUpgradeAction.Default;

            //State machine
            switch (PlayerState)
            {
                case ActionTypes.MoveAction: //moving action
                    {
                        switch (PlayerSubMoveState)
                        {
                            case SubMoveAction.MoveToRessource:
                                {
                                    action = AIHelper.CreateMoveAction(new Point(gameInfo.Player.Position.X + 1, gameInfo.Player.Position.Y));
                                    break;
                                    if (gameInfo.Player.CarriedResources >= gameInfo.Player.CarryingCapacity)
                                    {
                                        PlayerSubMoveState = SubMoveAction.ReturnToHouse;
                                        break;
                                    }
                                    else
                                    {
                                        double distance = 0;
                                        double lowestDistance = Int32.MaxValue;
                                        Point positionToMove = new Point(0, 0);
                                        for (int i = 0; i < carte.GetLength(0); i++)
                                        {
                                            for (int j = 0; j < carte.GetLength(1); j++)
                                            {
                                                if (carte[i, j].C == (byte)TileType.R)
                                                {
                                                    distance = Math.Sqrt(Math.Pow(carte[i, j].X - gameInfo.Player.Position.X, 2) + Math.Pow(carte[i, j].Y - gameInfo.Player.Position.Y, 2));
                                                    if (distance < lowestDistance)
                                                    {
                                                        positionToMove = new Point(carte[i, j].X, carte[i, j].Y);
                                                        lowestDistance = distance;
                                                    }
                                                }
                                            }
                                        }
                                        if (positionToMove.X > gameInfo.Player.Position.X + 1)
                                        {
                                            if (carte[10, 9].C == (byte)TileType.L || carte[10, 9].C == (byte)TileType.W)
                                            {
                                                if (positionToMove.Y > gameInfo.Player.Position.Y + 1)
                                                {
                                                    if (carte[9, 10].C == (byte)TileType.L || carte[9, 10].C == (byte)TileType.W)
                                                    {
                                                        action = AIHelper.CreateMoveAction(new Point(gameInfo.Player.Position.X, gameInfo.Player.Position.Y - 1));
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        action = AIHelper.CreateMoveAction(new Point(gameInfo.Player.Position.X, gameInfo.Player.Position.Y + 1));
                                                        break;
                                                    }
                                                }
                                                else
                                                {
                                                    if (carte[9, 8].C == (byte)TileType.L || carte[9, 8].C == (byte)TileType.W)
                                                    {
                                                        action = AIHelper.CreateMoveAction(new Point(gameInfo.Player.Position.X, gameInfo.Player.Position.Y + 1));
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        action = AIHelper.CreateMoveAction(new Point(gameInfo.Player.Position.X, gameInfo.Player.Position.Y - 1));
                                                        break;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                action = AIHelper.CreateMoveAction(new Point(gameInfo.Player.Position.X + 1, gameInfo.Player.Position.Y));
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            if (positionToMove.X < gameInfo.Player.Position.X - 1)
                                            {
                                                if (carte[8, 9].C == (byte)TileType.L || carte[8, 9].C == (byte)TileType.W)
                                                {
                                                    if (positionToMove.Y > gameInfo.Player.Position.Y + 1)
                                                    {
                                                        if (carte[9, 10].C == (byte)TileType.L || carte[9, 10].C == (byte)TileType.W)
                                                        {
                                                            action = AIHelper.CreateMoveAction(new Point(gameInfo.Player.Position.X, gameInfo.Player.Position.Y - 1));
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            action = AIHelper.CreateMoveAction(new Point(gameInfo.Player.Position.X, gameInfo.Player.Position.Y + 1));
                                                            break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (carte[9, 8].C == (byte)TileType.L || carte[9, 8].C == (byte)TileType.W)
                                                        {
                                                            action = AIHelper.CreateMoveAction(new Point(gameInfo.Player.Position.X, gameInfo.Player.Position.Y + 1));
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            action = AIHelper.CreateMoveAction(new Point(gameInfo.Player.Position.X, gameInfo.Player.Position.Y - 1));
                                                            break;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    action = AIHelper.CreateMoveAction(new Point(gameInfo.Player.Position.X - 1, gameInfo.Player.Position.Y));
                                                }
                                                break;
                                            }
                                            else
                                            {
                                                if (gameInfo.Player.Position.X != positionToMove.X && gameInfo.Player.Position.Y == positionToMove.Y)
                                                {
                                                    //test
                                                    action = AIHelper.CreateCollectAction(positionToMove);
                                                    break;
                                                }
                                                else
                                                {
                                                    if (gameInfo.Player.Position.X == positionToMove.X && gameInfo.Player.Position.Y != positionToMove.Y)
                                                    {
                                                        if (positionToMove.Y > gameInfo.Player.Position.Y + 1)
                                                        {
                                                            action = AIHelper.CreateMoveAction(new Point(gameInfo.Player.Position.X, gameInfo.Player.Position.Y + 1));
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            if (positionToMove.Y < gameInfo.Player.Position.Y - 1)
                                                            {
                                                                action = AIHelper.CreateMoveAction(new Point(gameInfo.Player.Position.X, gameInfo.Player.Position.Y - 1));
                                                                break;
                                                            }
                                                            else
                                                            {
                                                                action = AIHelper.CreateCollectAction(positionToMove);
                                                                break;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (positionToMove.Y > gameInfo.Player.Position.Y)
                                                        {
                                                            if (carte[9,10].C == (byte)TileType.L || carte[9, 10].C == (byte)TileType.W)
                                                            {
                                                                if (carte[10, 9].C == (byte)TileType.L || carte[10, 9].C == (byte)TileType.W)
                                                                {
                                                                    action = AIHelper.CreateMoveAction(new Point(gameInfo.Player.Position.X - 1, gameInfo.Player.Position.Y));
                                                                    break;
                                                                }
                                                                else
                                                                {
                                                                    action = AIHelper.CreateMoveAction(new Point(gameInfo.Player.Position.X + 1, gameInfo.Player.Position.Y));
                                                                    break;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                action = AIHelper.CreateMoveAction(new Point(gameInfo.Player.Position.X, gameInfo.Player.Position.Y + 1));
                                                                break;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (carte[9, 8].C == (byte)TileType.L || carte[9, 8].C == (byte)TileType.W)
                                                            {
                                                                if (carte[10, 9].C == (byte)TileType.L || carte[10, 9].C == (byte)TileType.W)
                                                                {
                                                                    action = AIHelper.CreateMoveAction(new Point(gameInfo.Player.Position.X - 1, gameInfo.Player.Position.Y));
                                                                    break;
                                                                }
                                                                else
                                                                {
                                                                    action = AIHelper.CreateMoveAction(new Point(gameInfo.Player.Position.X + 1, gameInfo.Player.Position.Y));
                                                                    break;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                action = AIHelper.CreateMoveAction(new Point(gameInfo.Player.Position.X, gameInfo.Player.Position.Y - 1));
                                                                break;
                                                            }
                                                        }
                                                    }

                                                }
                                            }
                                        }
                                    }

                                    
                                }
                            case SubMoveAction.ReturnToHouse:
                                {
                                    if (gameInfo.Player.HouseLocation.X > gameInfo.Player.Position.X)
                                    {
                                        action = AIHelper.CreateMoveAction(new Point(gameInfo.Player.Position.X + 1, gameInfo.Player.Position.Y));
                                        break;
                                    }
                                    else
                                    {
                                        if (gameInfo.Player.HouseLocation.X < gameInfo.Player.Position.X)
                                        {
                                            action = AIHelper.CreateMoveAction(new Point(gameInfo.Player.Position.X - 1, gameInfo.Player.Position.Y));
                                            break;
                                        }
                                        else
                                        {
                                            if (gameInfo.Player.HouseLocation.Y > gameInfo.Player.Position.Y)
                                            {
                                                action = AIHelper.CreateMoveAction(new Point(gameInfo.Player.Position.X, gameInfo.Player.Position.Y + 1));
                                                break;
                                            }
                                            else
                                            {
                                                if (gameInfo.Player.HouseLocation.Y < gameInfo.Player.Position.Y)
                                                {
                                                    action = AIHelper.CreateMoveAction(new Point(gameInfo.Player.Position.X, gameInfo.Player.Position.Y - 1));
                                                    break;
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            case SubMoveAction.MoveToPlayer:
                                {
                                    double lowestDistance = Int32.MaxValue;
                                    Point playerLocation = new Point(0, 0);
                                    foreach(KeyValuePair<string,PlayerInfo> p in gameInfo.OtherPlayers)
                                    {
                                        double distance = Math.Sqrt(Math.Pow(p.Value.Position.X - gameInfo.Player.Position.X,2) + Math.Pow(p.Value.Position.Y - gameInfo.Player.Position.Y, 2));
                                        if (distance < lowestDistance)
                                        {
                                            lowestDistance = distance;
                                            playerLocation = p.Value.Position;
                                        }
                                    }
                                    //pathfinding with playerlocation
                                    //if arrived action = attackAction
                                    break;
                                }
                            case SubMoveAction.Default:
                                {
                                    //code
                                    break;
                                }
                        }
                        break;
                    }
                case ActionTypes.CollectAction: //collect action
                    {
                        switch (PlayerSubCollectState)
                        {
                            case SubCollectAction.CollectRessource:
                                {
                                    if (gameInfo.Player.CarriedResources >= gameInfo.Player.CarryingCapacity)
                                    {
                                        PlayerState = ActionTypes.MoveAction;
                                        PlayerSubMoveState = SubMoveAction.ReturnToHouse;
                                    }
                                    else
                                    {
                                        Point ressourcePosition = new Point(0, 0);

                                        if (carte[gameInfo.Player.Position.X + 1, gameInfo.Player.Position.Y].C == (byte)TileType.R)
                                        {
                                            ressourcePosition = new Point(gameInfo.Player.Position.X + 1, gameInfo.Player.Position.Y);
                                        }
                                        if (carte[gameInfo.Player.Position.X - 1, gameInfo.Player.Position.Y].C == (byte)TileType.R)
                                        {
                                            ressourcePosition = new Point(gameInfo.Player.Position.X - 1, gameInfo.Player.Position.Y);
                                        }
                                        if (carte[gameInfo.Player.Position.X, gameInfo.Player.Position.Y + 1].C == (byte)TileType.R)
                                        {
                                            ressourcePosition = new Point(gameInfo.Player.Position.X, gameInfo.Player.Position.Y + 1);
                                        }
                                        if (carte[gameInfo.Player.Position.X, gameInfo.Player.Position.Y - 1].C == (byte)TileType.R)
                                        {
                                            ressourcePosition = new Point(gameInfo.Player.Position.X, gameInfo.Player.Position.Y - 1);
                                        }
                                        else
                                        {
                                            //PlayerState = ActionTypes.DefaultAction;
                                            //PlayerSubMoveState = SubMoveAction.Default;
                                            break;
                                        }


                                        action = AIHelper.CreateCollectAction(ressourcePosition);
                                    }
                                    break;
                                }
                            case SubCollectAction.DropRessourceToHouse:
                                {
                                    //code
                                    AIHelper.CreateCollectAction(gameInfo.Player.Position);
                                    PlayerState = ActionTypes.DefaultAction;
                                    PlayerSubMoveState = SubMoveAction.Default;
                                    break;
                                }
                            case SubCollectAction.Default:
                                {
                                    //code
                                    break;
                                }
                        }
                        break;
                    }
                case ActionTypes.AttackAction:
                    {
                        for (int i = -1 + gameInfo.Player.Position.X; i < 2 + gameInfo.Player.Position.X; i++)
                        {
                            for (int j = -1 + gameInfo.Player.Position.Y; j < 2 + gameInfo.Player.Position.Y; j++)
                            {
                                foreach(KeyValuePair<string,PlayerInfo> p in gameInfo.OtherPlayers)
                                {
                                    Point attackpoint = new Point(carte[i, j].X, carte[i, j].Y);
                                    if (p.Value.Position == attackpoint)
                                    {
                                        action = AIHelper.CreateAttackAction(attackpoint);
                                        break;
                                    }
                                }
                            }
                        }
                        PlayerState = ActionTypes.MoveAction;
                        PlayerSubMoveState = SubMoveAction.MoveToPlayer;
                        break;
                    }
                case ActionTypes.DefaultAction:
                    {
                        break;
                    }
            }

            void ValidateFuturAction()
            {
                if (gameInfo.Player.CarriedResources >= gameInfo.Player.CarryingCapacity)
                {
                    PlayerState = ActionTypes.MoveAction;
                    PlayerSubMoveState = SubMoveAction.ReturnToHouse;
                }
                else
                {
                    PlayerState = ActionTypes.MoveAction;
                    PlayerSubMoveState = SubMoveAction.MoveToRessource;
                }
            }

            return action;
        }
    }
}

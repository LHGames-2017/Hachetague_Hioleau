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
            ReturnToHouse
        } //sub move action
        enum SubCollectAction
        {
            Default,
            CollectRessource,
            DropRessourceToHouse
        } //sub collect action


        [HttpPost]
        public string Index([FromForm]string map)
        {
            GameInfo gameInfo = JsonConvert.DeserializeObject<GameInfo>(map);
            var carte = AIHelper.DeserializeMap(gameInfo.CustomSerializedMap);

            // INSERT AI CODE HERE.
            ActionTypes PlayerState = ActionTypes.DefaultAction;
            SubMoveAction PlayerSubMoveState = SubMoveAction.Default;
            SubCollectAction PlayerSubCollectState = SubCollectAction.Default;

            //State machine
            switch (PlayerState)
            {
                case ActionTypes.MoveAction: //moving action
                    {
                        switch (PlayerSubMoveState)
                        {
                            case SubMoveAction.MoveToRessource:
                                {
                                    double distance = 0;
                                    double lowestDistance = Int32.MaxValue;
                                    Point positionToMove = new Point(0, 0);
                                    for (int i = 0; i < carte.GetLength(0); i++)
                                    {
                                        for (int j = 0; j < carte.GetLength(1); j++)
                                        {
                                            if (carte[i, j].C == 'R')
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
                                    //pathfiding on passe position to move
                                    break;
                                }
                            case SubMoveAction.ReturnToHouse:
                                {
                                    //path fiding code with gameInfo.Player.houseposition
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

                                        if (carte[gameInfo.Player.Position.X + 1, gameInfo.Player.Position.Y].C == 'R')
                                        {
                                            ressourcePosition = new Point(gameInfo.Player.Position.X + 1, gameInfo.Player.Position.Y);
                                        }
                                        if (carte[gameInfo.Player.Position.X - 1, gameInfo.Player.Position.Y].C == 'R')
                                        {
                                            ressourcePosition = new Point(gameInfo.Player.Position.X - 1, gameInfo.Player.Position.Y);
                                        }
                                        if (carte[gameInfo.Player.Position.X, gameInfo.Player.Position.Y + 1].C == 'R')
                                        {
                                            ressourcePosition = new Point(gameInfo.Player.Position.X, gameInfo.Player.Position.Y + 1);
                                        }
                                        if (carte[gameInfo.Player.Position.X, gameInfo.Player.Position.Y - 1].C == 'R')
                                        {
                                            ressourcePosition = new Point(gameInfo.Player.Position.X, gameInfo.Player.Position.Y - 1);
                                        }
                                        else
                                        {
                                            PlayerState = ActionTypes.DefaultAction;
                                            PlayerSubMoveState = SubMoveAction.Default;
                                            break;
                                        }


                                        AIHelper.CreateCollectAction(ressourcePosition);
                                    }
                                    break;
                                }
                            case SubCollectAction.DropRessourceToHouse:
                                {
                                    //code
                                    AIHelper.CreateCollectAction(gameInfo.Player.Position);
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
                case ActionTypes.DefaultAction:
                    {
                        if (gameInfo.Player.CarriedResources < gameInfo.Player.CarryingCapacity)
                        {
                            PlayerState = ActionTypes.MoveAction;
                            PlayerSubMoveState = SubMoveAction.MoveToRessource;
                        }
                        break;
                    }
            }


            string action = AIHelper.CreateMoveAction(gameInfo.Player.Position);
            return action;
        }
    }
}

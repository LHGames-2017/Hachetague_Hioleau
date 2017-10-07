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
            switch(PlayerState)
            {
                case ActionTypes.MoveAction: //moving action
                {
                    switch(PlayerSubMoveState)
                    {
                        case SubMoveAction.MoveToRessource:
                        {
                            //code
                            break;
                        }
                        case SubMoveAction.ReturnToHouse:
                        {
                            //code
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
                    switch(PlayerSubCollectState)
                    {
                        case SubCollectAction.CollectRessource:
                        {
                            //code
                            break;
                        }
                        case SubCollectAction.DropRessourceToHouse:
                        {
                            //code
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
                    if (gameInfo.Player.CarriedResources <= gameInfo.Player.CarryingCapacity)
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

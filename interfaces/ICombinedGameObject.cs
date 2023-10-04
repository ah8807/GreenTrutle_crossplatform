using System.Collections.Generic;
using GreenTrutle_crossplatform.scene;

namespace GreenTrutle_crossplatform.interfaces;

public interface ICombinedGameObject
{
    List<DrawableGameObject> game_objects { get; set; }
}
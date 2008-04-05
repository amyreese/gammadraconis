--[[ 
  Asteroid Definitions 
  ----
  Asteroid800A
  ]]--
  
-- AsteroidBase
base = GameObject()
base.dragR = 0
base.dragL = 0
base.mass = 10000
base.size = 100

-- Asteroid800A
fbx = FBXModel("Resources/Models/Asteroid800A", "", 200)

roid = base:clone()
roid.models:Add(fbx)

Proto.thing:Add("Asteroid800A", roid)

-- Asteroid800B
fbx = FBXModel("Resources/Models/Asteroid800B", "", 200)

roid = base:clone()
roid.models:Add(fbx)

Proto.thing:Add("Asteroid800B", roid)
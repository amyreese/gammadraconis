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

-- Asteroid800A
fbx = FBXModel("Resources/Models/Asteroid800A", "", 200)

print('here')
roid = base:clone()
print('here')
roid.models:Add(fbx)
print('here')

Proto.thing:Add("Asteroid800A", roid)
print('here')
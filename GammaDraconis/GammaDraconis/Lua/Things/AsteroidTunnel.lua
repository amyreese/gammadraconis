roid = GameObject()
roid.dragR = 0
roid.dragL = 0
roid.mass = 100000
roid.size = 1500
roid.maxHealth = 3000
roid.immobile = true
roid.invincible = true

fbx = FBXModel("Resources/Models/AsteroidTunnel", "", 10)

roid.models:Add(fbx)

Proto.thing:Add("AsteroidTunnel", roid)
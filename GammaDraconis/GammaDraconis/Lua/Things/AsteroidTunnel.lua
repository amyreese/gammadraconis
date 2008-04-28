roid = GameObject()
roid.dragR = 0
roid.dragL = 0
roid.mass = 100000
roid.size = 750
roid.maxHealth = 3000
roid.immobile = true
roid.invincible = true

fbx = FBXModel("Resources/Models/AsteroidTunnel", "", 5)

roid.models:Add(fbx)

Proto.thing:Add("AsteroidTunnel", roid)
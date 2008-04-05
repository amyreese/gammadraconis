
shipModel = FBXModel("Resources/Models/Raptor")
shieldModel = FBXModel("Resources/Models/Shield", "", 10)
shieldModel.visible = false

mountR = MountPoint()
mountR.location = Coords(0.3, 0, 0)
mountR.weapon = Proto.getWeapon("Cannon")

mountL = MountPoint()
mountL.location = Coords(-0.3, 0, 0)
mountL.weapon = Proto.getWeapon("Cannon")

ship = GameObject()

ship.mass = 800

ship.rateL = 220
ship.dragL = 1.2

ship.rateR = 100
ship.dragR = 3

ship.models:Add(shipModel)
ship.shieldModel = shieldModel

ship.mounts:Add(mountR)
ship.mounts:Add(mountL)

Proto.ship:Add("Raptor", ship)

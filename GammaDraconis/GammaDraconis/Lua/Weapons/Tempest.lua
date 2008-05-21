
weapon = Weapon()
weapon.type = W_TYPE.SECONDARY
weapon.cooldown = 400
weapon.ammoMax = 10
weapon.fireFrom = Coords(0,-7,0)
weapon.fireSFX = "drop_mine"
weapon.impactSFX = "explosion"
weapon.bullet.onDeathSound = "explosion"

weapon.bullet.damage = 50
weapon.bullet.rateL = 1000
weapon.bullet.dragL = 0
weapon.bullet.dragR = 0.5
weapon.bullet.size = 20
weapon.bullet.timeToLive = 15
weapon.bullet.models:Add(FBXModel("Resources/Models/Rocket", "", 1.0));
weapon.bullet.models:Add(FBXModel("Resources/Models/Rocket2", "", 1.0));

Proto.weapon:Add("Tempest", weapon)
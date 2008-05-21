
weapon = Weapon()
weapon.type = W_TYPE.SECONDARY
weapon.cooldown = 2000
weapon.ammoMax = 10
weapon.fireFrom = Coords(0,-7,0)
weapon.fireSFX = "drop_mine"
weapon.impactSFX = "explosion"
weapon.bullet.onDeathSound = "explosion"
weapon.bullet.explosion = Explosion()
weapon.bullet.explosion.particles = 70
weapon.bullet.explosion.power = 10

weapon.bullet.damage = 0
weapon.bullet.rateL = 0
weapon.bullet.dragL = 0.9
weapon.bullet.dragR = 0.4
weapon.bullet.size = 70
weapon.bullet.timeToLive = 15
weapon.bullet.models:Add(FBXModel("Resources/Models/Mine", "", 1.0));
weapon.bullet.models:Add(FBXModel("Resources/Models/Mine2", "", 1.0));

Proto.weapon:Add("Spike", weapon)
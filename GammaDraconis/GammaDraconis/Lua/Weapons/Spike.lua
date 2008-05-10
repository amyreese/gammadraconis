
weapon = Weapon()
weapon.type = W_TYPE.SECONDARY
weapon.cooldown = 2000
weapon.ammoMax = 10;

weapon.bullet.damage = 5000
weapon.bullet.rateL = 0
weapon.bullet.dragL = 0.9
weapon.bullet.dragR = 0.4
weapon.bullet.timeToLive = 30
weapon.bullet.models:Add(FBXModel("Resources/Models/Mine", "", 1.0));
weapon.bullet.models:Add(FBXModel("Resources/Models/Mine2", "", 1.0));

Proto.weapon:Add("Spike", weapon)
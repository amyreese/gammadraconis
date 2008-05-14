dust = GameObject()
model = FBXModel("Resources/Models/Particle", "", 0.25)
model.lighted = false
dust.models:Add(model)
Proto.thing:Add("Dust", dust)
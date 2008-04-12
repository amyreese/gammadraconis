thing = GameObject()
thing.mass = 0
thing.rateL = 0
thing.rateR = 0
thing.models:Add(FBXModel("Resources/Models/Shell"))
Proto.thing:Add("Dummy", thing)
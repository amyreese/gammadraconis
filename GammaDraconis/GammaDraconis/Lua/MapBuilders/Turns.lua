function AddDownTurn( path, xoffset, yoffset, zoffset, trackAttributes )
	for i = 0, 90  - trackAttributes.degreesBetweenCheckpoints, trackAttributes.degreesBetweenCheckpoints do
		local rad = MathHelper.ToRadians(i)
		local y = -MSMath.Cos(rad)
		local z = MSMath.Sin(rad)
		table.insert( path, {x=(0) + xoffset, y=(y*trackAttributes.radius) + yoffset, z=(z*trackAttributes.radius) + zoffset, pitch=MSMath.PI - rad, yaw=0, roll=0} )
	end
	
	return path
end
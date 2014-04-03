
using(console);

SystemNameGenerator = 
{
	x: ["Xen ", "De ", "Vis "],
	a: ["For", "Ter", "Mer", "Gat", "Or", "In", "Tes", "Nor", "Bar", "Car", "Ton", "Nos", "Strot", "Cos", "Zon", "Was", "Gan"],
	b: ["man", "con", "ten", "den", "on", "port", "gat", "den"],
	c: ["ra", "sima", "bata", "tuma", "la", "sa"],
	d: ["ama", "et", "ar", "enn", "acor", "oden"],
	e: ["sa", "zed", "dar", "tenn", "car", "den"],
	z: [" Alpha"," Beta", " Gamma", " Theta"],

	getBlock: function(arr)
	{
		return arr[MathExt.RandRange(0,arr.length)];
	},

	generatePattern: function()
	{
		var str = "";
		if(MathExt.RandDouble() < 1/6)
		{
			str += "x";
		}
		str += 'a';
		if(MathExt.RandDouble() > 0.5)
		{
			str += (MathExt.RandDouble() < 0.25) ? 'b' : '';
			str += 'd';
		}
		else
		{
			str += (MathExt.RandDouble() < 0.25) ? 'c' : '';
			str += 'e';
		}
		if(MathExt.RandDouble() < 1/6)
		{
			str += 'z';
		}
		
		return str;
	},
	
	
	parsePattern: function(pat)
	{
		var temp = '';
		
		for(var i=0; i < pat.length; i++)
		{
			temp += this.getBlock(this[pat[i]]);
		}
		
		return temp;
	},
	
	GetName: function()
	{
		return this.parsePattern(this.generatePattern());
	},

};
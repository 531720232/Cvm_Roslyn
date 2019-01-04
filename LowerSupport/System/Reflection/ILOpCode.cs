namespace System.Reflection.Metadata
{
	public enum ILOpCode : ushort
	{
		/// <returns></returns>
		Nop = 0,
		/// <returns></returns>
		Break = 1,
		/// <returns></returns>
		Ldarg_0 = 2,
		/// <returns></returns>
		Ldarg_1 = 3,
		/// <returns></returns>
		Ldarg_2 = 4,
		/// <returns></returns>
		Ldarg_3 = 5,
		/// <returns></returns>
		Ldloc_0 = 6,
		/// <returns></returns>
		Ldloc_1 = 7,
		/// <returns></returns>
		Ldloc_2 = 8,
		/// <returns></returns>
		Ldloc_3 = 9,
		/// <returns></returns>
		Stloc_0 = 10,
		/// <returns></returns>
		Stloc_1 = 11,
		/// <returns></returns>
		Stloc_2 = 12,
		/// <returns></returns>
		Stloc_3 = 13,
		/// <returns></returns>
		Ldarg_s = 14,
		/// <returns></returns>
		Ldarga_s = 0xF,
		/// <returns></returns>
		Starg_s = 0x10,
		/// <returns></returns>
		Ldloc_s = 17,
		/// <returns></returns>
		Ldloca_s = 18,
		/// <returns></returns>
		Stloc_s = 19,
		/// <returns></returns>
		Ldnull = 20,
		/// <returns></returns>
		Ldc_i4_m1 = 21,
		/// <returns></returns>
		Ldc_i4_0 = 22,
		/// <returns></returns>
		Ldc_i4_1 = 23,
		/// <returns></returns>
		Ldc_i4_2 = 24,
		/// <returns></returns>
		Ldc_i4_3 = 25,
		/// <returns></returns>
		Ldc_i4_4 = 26,
		/// <returns></returns>
		Ldc_i4_5 = 27,
		/// <returns></returns>
		Ldc_i4_6 = 28,
		/// <returns></returns>
		Ldc_i4_7 = 29,
		/// <returns></returns>
		Ldc_i4_8 = 30,
		/// <returns></returns>
		Ldc_i4_s = 0x1F,
		/// <returns></returns>
		Ldc_i4 = 0x20,
		/// <returns></returns>
		Ldc_i8 = 33,
		/// <returns></returns>
		Ldc_r4 = 34,
		/// <returns></returns>
		Ldc_r8 = 35,
		/// <returns></returns>
		Dup = 37,
		/// <returns></returns>
		Pop = 38,
		/// <returns></returns>
		Jmp = 39,
		/// <returns></returns>
		Call = 40,
		/// <returns></returns>
		Calli = 41,
		/// <returns></returns>
		Ret = 42,
		/// <returns></returns>
		Br_s = 43,
		/// <returns></returns>
		Brfalse_s = 44,
		/// <returns></returns>
		Brtrue_s = 45,
		/// <returns></returns>
		Beq_s = 46,
		/// <returns></returns>
		Bge_s = 47,
		/// <returns></returns>
		Bgt_s = 48,
		/// <returns></returns>
		Ble_s = 49,
		/// <returns></returns>
		Blt_s = 50,
		/// <returns></returns>
		Bne_un_s = 51,
		/// <returns></returns>
		Bge_un_s = 52,
		/// <returns></returns>
		Bgt_un_s = 53,
		/// <returns></returns>
		Ble_un_s = 54,
		/// <returns></returns>
		Blt_un_s = 55,
		/// <returns></returns>
		Br = 56,
		/// <returns></returns>
		Brfalse = 57,
		/// <returns></returns>
		Brtrue = 58,
		/// <returns></returns>
		Beq = 59,
		/// <returns></returns>
		Bge = 60,
		/// <returns></returns>
		Bgt = 61,
		/// <returns></returns>
		Ble = 62,
		/// <returns></returns>
		Blt = 0x3F,
		/// <returns></returns>
		Bne_un = 0x40,
		/// <returns></returns>
		Bge_un = 65,
		/// <returns></returns>
		Bgt_un = 66,
		/// <returns></returns>
		Ble_un = 67,
		/// <returns></returns>
		Blt_un = 68,
		/// <returns></returns>
		Switch = 69,
		/// <returns></returns>
		Ldind_i1 = 70,
		/// <returns></returns>
		Ldind_u1 = 71,
		/// <returns></returns>
		Ldind_i2 = 72,
		/// <returns></returns>
		Ldind_u2 = 73,
		/// <returns></returns>
		Ldind_i4 = 74,
		/// <returns></returns>
		Ldind_u4 = 75,
		/// <returns></returns>
		Ldind_i8 = 76,
		/// <returns></returns>
		Ldind_i = 77,
		/// <returns></returns>
		Ldind_r4 = 78,
		/// <returns></returns>
		Ldind_r8 = 79,
		/// <returns></returns>
		Ldind_ref = 80,
		/// <returns></returns>
		Stind_ref = 81,
		/// <returns></returns>
		Stind_i1 = 82,
		/// <returns></returns>
		Stind_i2 = 83,
		/// <returns></returns>
		Stind_i4 = 84,
		/// <returns></returns>
		Stind_i8 = 85,
		/// <returns></returns>
		Stind_r4 = 86,
		/// <returns></returns>
		Stind_r8 = 87,
		/// <returns></returns>
		Add = 88,
		/// <returns></returns>
		Sub = 89,
		/// <returns></returns>
		Mul = 90,
		/// <returns></returns>
		Div = 91,
		/// <returns></returns>
		Div_un = 92,
		/// <returns></returns>
		Rem = 93,
		/// <returns></returns>
		Rem_un = 94,
		/// <returns></returns>
		And = 95,
		/// <returns></returns>
		Or = 96,
		/// <returns></returns>
		Xor = 97,
		/// <returns></returns>
		Shl = 98,
		/// <returns></returns>
		Shr = 99,
		/// <returns></returns>
		Shr_un = 100,
		/// <returns></returns>
		Neg = 101,
		/// <returns></returns>
		Not = 102,
		/// <returns></returns>
		Conv_i1 = 103,
		/// <returns></returns>
		Conv_i2 = 104,
		/// <returns></returns>
		Conv_i4 = 105,
		/// <returns></returns>
		Conv_i8 = 106,
		/// <returns></returns>
		Conv_r4 = 107,
		/// <returns></returns>
		Conv_r8 = 108,
		/// <returns></returns>
		Conv_u4 = 109,
		/// <returns></returns>
		Conv_u8 = 110,
		/// <returns></returns>
		Callvirt = 111,
		/// <returns></returns>
		Cpobj = 112,
		/// <returns></returns>
		Ldobj = 113,
		/// <returns></returns>
		Ldstr = 114,
		/// <returns></returns>
		Newobj = 115,
		/// <returns></returns>
		Castclass = 116,
		/// <returns></returns>
		Isinst = 117,
		/// <returns></returns>
		Conv_r_un = 118,
		/// <returns></returns>
		Unbox = 121,
		/// <returns></returns>
		Throw = 122,
		/// <returns></returns>
		Ldfld = 123,
		/// <returns></returns>
		Ldflda = 124,
		/// <returns></returns>
		Stfld = 125,
		/// <returns></returns>
		Ldsfld = 126,
		/// <returns></returns>
		Ldsflda = 0x7F,
		/// <returns></returns>
		Stsfld = 0x80,
		/// <returns></returns>
		Stobj = 129,
		/// <returns></returns>
		Conv_ovf_i1_un = 130,
		/// <returns></returns>
		Conv_ovf_i2_un = 131,
		/// <returns></returns>
		Conv_ovf_i4_un = 132,
		/// <returns></returns>
		Conv_ovf_i8_un = 133,
		/// <returns></returns>
		Conv_ovf_u1_un = 134,
		/// <returns></returns>
		Conv_ovf_u2_un = 135,
		/// <returns></returns>
		Conv_ovf_u4_un = 136,
		/// <returns></returns>
		Conv_ovf_u8_un = 137,
		/// <returns></returns>
		Conv_ovf_i_un = 138,
		/// <returns></returns>
		Conv_ovf_u_un = 139,
		/// <returns></returns>
		Box = 140,
		/// <returns></returns>
		Newarr = 141,
		/// <returns></returns>
		Ldlen = 142,
		/// <returns></returns>
		Ldelema = 143,
		/// <returns></returns>
		Ldelem_i1 = 144,
		/// <returns></returns>
		Ldelem_u1 = 145,
		/// <returns></returns>
		Ldelem_i2 = 146,
		/// <returns></returns>
		Ldelem_u2 = 147,
		/// <returns></returns>
		Ldelem_i4 = 148,
		/// <returns></returns>
		Ldelem_u4 = 149,
		/// <returns></returns>
		Ldelem_i8 = 150,
		/// <returns></returns>
		Ldelem_i = 151,
		/// <returns></returns>
		Ldelem_r4 = 152,
		/// <returns></returns>
		Ldelem_r8 = 153,
		/// <returns></returns>
		Ldelem_ref = 154,
		/// <returns></returns>
		Stelem_i = 155,
		/// <returns></returns>
		Stelem_i1 = 156,
		/// <returns></returns>
		Stelem_i2 = 157,
		/// <returns></returns>
		Stelem_i4 = 158,
		/// <returns></returns>
		Stelem_i8 = 159,
		/// <returns></returns>
		Stelem_r4 = 160,
		/// <returns></returns>
		Stelem_r8 = 161,
		/// <returns></returns>
		Stelem_ref = 162,
		/// <returns></returns>
		Ldelem = 163,
		/// <returns></returns>
		Stelem = 164,
		/// <returns></returns>
		Unbox_any = 165,
		/// <returns></returns>
		Conv_ovf_i1 = 179,
		/// <returns></returns>
		Conv_ovf_u1 = 180,
		/// <returns></returns>
		Conv_ovf_i2 = 181,
		/// <returns></returns>
		Conv_ovf_u2 = 182,
		/// <returns></returns>
		Conv_ovf_i4 = 183,
		/// <returns></returns>
		Conv_ovf_u4 = 184,
		/// <returns></returns>
		Conv_ovf_i8 = 185,
		/// <returns></returns>
		Conv_ovf_u8 = 186,
		/// <returns></returns>
		Refanyval = 194,
		/// <returns></returns>
		Ckfinite = 195,
		/// <returns></returns>
		Mkrefany = 198,
		/// <returns></returns>
		Ldtoken = 208,
		/// <returns></returns>
		Conv_u2 = 209,
		/// <returns></returns>
		Conv_u1 = 210,
		/// <returns></returns>
		Conv_i = 211,
		/// <returns></returns>
		Conv_ovf_i = 212,
		/// <returns></returns>
		Conv_ovf_u = 213,
		/// <returns></returns>
		Add_ovf = 214,
		/// <returns></returns>
		Add_ovf_un = 215,
		/// <returns></returns>
		Mul_ovf = 216,
		/// <returns></returns>
		Mul_ovf_un = 217,
		/// <returns></returns>
		Sub_ovf = 218,
		/// <returns></returns>
		Sub_ovf_un = 219,
		/// <returns></returns>
		Endfinally = 220,
		/// <returns></returns>
		Leave = 221,
		/// <returns></returns>
		Leave_s = 222,
		/// <returns></returns>
		Stind_i = 223,
		/// <returns></returns>
		Conv_u = 224,
		/// <returns></returns>
		Arglist = 65024,
		/// <returns></returns>
		Ceq = 65025,
		/// <returns></returns>
		Cgt = 65026,
		/// <returns></returns>
		Cgt_un = 65027,
		/// <returns></returns>
		Clt = 65028,
		/// <returns></returns>
		Clt_un = 65029,
		/// <returns></returns>
		Ldftn = 65030,
		/// <returns></returns>
		Ldvirtftn = 65031,
		/// <returns></returns>
		Ldarg = 65033,
		/// <returns></returns>
		Ldarga = 65034,
		/// <returns></returns>
		Starg = 65035,
		/// <returns></returns>
		Ldloc = 65036,
		/// <returns></returns>
		Ldloca = 65037,
		/// <returns></returns>
		Stloc = 65038,
		/// <returns></returns>
		Localloc = 65039,
		/// <returns></returns>
		Endfilter = 65041,
		/// <returns></returns>
		Unaligned = 65042,
		/// <returns></returns>
		Volatile = 65043,
		/// <returns></returns>
		Tail = 65044,
		/// <returns></returns>
		Initobj = 65045,
		/// <returns></returns>
		Constrained = 65046,
		/// <returns></returns>
		Cpblk = 65047,
		/// <returns></returns>
		Initblk = 65048,
		/// <returns></returns>
		Rethrow = 65050,
		/// <returns></returns>
		Sizeof = 65052,
		/// <returns></returns>
		Refanytype = 65053,
		/// <returns></returns>
		Readonly = 65054
	}
}

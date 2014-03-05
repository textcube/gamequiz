using System;
using System.Collections.Generic;
 
public static class ListExtensions {
	public static void Shuffle<T>(this IList<T> list) {
		var randomNumber = new Random(DateTime.Now.Millisecond);
		var n = list.Count;
		while (n > 1) {
			n--;
			var k = randomNumber.Next(n + 1);
			var value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
	}
}

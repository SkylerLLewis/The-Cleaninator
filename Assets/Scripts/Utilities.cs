using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities {

    public static T Choice<T>(T [] opt) {
        return opt[Random.Range(0, opt.Length)];
    }
    public static T Choice<T>(List<T> opt) {
        return opt[Random.Range(0, opt.Count)];
    }
    public static T Choice<T>(T a, T b) {
        if (Random.Range(0,2) == 1) {
            return a;
        } else {
            return b;
        }
    }

    public static List<T> Sample<T>(int count, in List<T> list) {
        List<T> newList = new List<T>();
        for (int i=0; i<list.Count; i++) {
            if (Random.value < (float)count/(list.Count-i)) {
                newList.Add(list[i]);
                count--;
                if (count == 0) break;
            }
        }
        return newList;
    }
}
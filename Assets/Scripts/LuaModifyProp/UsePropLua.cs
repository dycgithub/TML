using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;


    [LuaCallCSharp]
    public class UsePropLua : MonoBehaviour
    {
        public TextAsset luaScript;
        private LuaEnv luaEnv = new();
        
        private Action luaPropFunctionTest;

        private LuaTable scriptScopeTable; //lua脚本域
        
        public void Start()
        {
            
            scriptScopeTable = luaEnv.NewTable(); //为每个脚本设置一个独立的脚本域
            using (LuaTable meta = luaEnv.NewTable())//设置其元表的 __index, 使其能够访问全局变量
            {
                meta.Set("__index", luaEnv.Global);
                scriptScopeTable.SetMetaTable(meta);
            }
            luaEnv.DoString(luaScript.text, luaScript.name, scriptScopeTable); //执行脚本
            
            scriptScopeTable.Get("PropFunctionTest", out luaPropFunctionTest);
            Debug.Log("luaPropFunctionTest");
        }
        
        public void MyCallLuaPropFunction()
        {
            if (luaPropFunctionTest != null)
            {
                print("CallLuaPropFunction");
                luaPropFunctionTest();
            }
        }

    }

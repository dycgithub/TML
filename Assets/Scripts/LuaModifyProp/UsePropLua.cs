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

        private Action luaStart;
        private Action luaUpdate;
        private Action luaOnDestroy;
        private Action luaPropFunctionTest;

        private LuaTable scriptScopeTable; //lua脚本域

        private void Awake()
        {
            scriptScopeTable = luaEnv.NewTable(); //为每个脚本设置一个独立的脚本域
            using (LuaTable meta = luaEnv.NewTable())//设置其元表的 __index, 使其能够访问全局变量
            {
                meta.Set("__index", luaEnv.Global);
                scriptScopeTable.SetMetaTable(meta);
            }
            luaEnv.DoString(luaScript.text, luaScript.name, scriptScopeTable); //执行脚本

            Action luaAwake = scriptScopeTable.Get<Action>("awake");
            scriptScopeTable.Get("start", out luaStart); //从Lua脚本域中获取定义的函数
            scriptScopeTable.Get("update", out luaUpdate);

            scriptScopeTable.Get("PropFunctionTest", out luaPropFunctionTest);
            if (luaAwake != null)
            {
                luaAwake();
            }

        }

        private void Start()
        {
            if (luaStart != null)
            {
                luaStart();
            }
        }

        private void Update()
        {
            if (luaUpdate != null)
            {
                luaUpdate();
            }

        }


        public void CallLuaPropFunction()
        {
            if (luaPropFunctionTest != null)
            {
                print("CallLuaPropFunction");
                luaPropFunctionTest();
            }
        }

    }

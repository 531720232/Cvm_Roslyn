# Cvm_Roslyn


##cvm是什么?
一个静态DLL编译器
简而言之就是读取*.cs在游戏过程中生成DLL，以达到一次打包一劳永逸(大概吧)


##生成的DLL可以干什么？
可以动态利用ILRuntime实现热更

##最低Net4.0需求(NET3.5有些特性没法读取)

##有可能有些零零碎碎的问题
##使用方法

```csharp
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class B1 : MonoBehaviour {

    public UnityEngine.UI.Text Text;
    public TextAsset cs_cs;
    // Use this for initialization
	void Start () {
        try
        {
            string text = cs_cs.text;

            CVM.GlobalDefine.Instance.InDebug();
            SyntaxTree tree = SyntaxFactory.ParseSyntaxTree(text);
            CSharpCompilation compilation = CSharpCompilation.Create(
                "q1.dll",
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary),
                syntaxTrees: new[] { tree },
                 references: new[] { MetadataReference.CreateFromFile("this is not use") }
                );

            using (MemoryStream stream = new MemoryStream())

            {


                Microsoft.CodeAnalysis.Emit.EmitResult compileResult = compilation.Emit(stream);

                var assem = System.Reflection.Assembly.Load(stream.GetBuffer());//
                stream.Position = 0;
                var all_bytes = assem.GetTypes();
                Debug.Log("发现类" + all_bytes.Length);
                if (Text != null)
                {
                    Text.text = "发现类" + all_bytes.Length;
                }
            }
        }
        catch(System.Exception e)
        {
            if (Text != null)
            {
                Text.text =e.ToString() ;
            }
        }

    }

    // Update is called once per frame
    void Update () {
		
	}
}

```


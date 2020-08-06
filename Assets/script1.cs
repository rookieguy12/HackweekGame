using UnityEngine;
using UnityEngine.UI;
public class script1 : MonoBehaviour
{
    delegate int NumberChanger(int n);
    private void Start()
    {
        run();
    }
    
        static int num = 10;
        public static int AddNum(int p)
        {
            num += p;
            return num;
        }

        public static int MultNum(int q)
        {
            num *= q;
            return num;
        }
        public static int getNum()
        {
            return num;
        }

        static void run()
        {
            // 创建委托实例
            NumberChanger nc;
            NumberChanger nc1 = new NumberChanger(AddNum);
            NumberChanger nc2 = new NumberChanger(MultNum);
            nc = nc1;
            nc += nc2;
            // 调用多播
            nc(5);
            print("Value of Num: {0}" +  getNum());
        }
    }

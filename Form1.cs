using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace WindowsFormsApp1
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			GET_DB_FROMFILEtag_use_lsit();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			MAKE_FILEtag_use_lsit();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			GET_DB_FROMFILEtag_use_lsit();


		}

		private void button3_Click(object sender, EventArgs e)
		{
			MAK_DB_FINAL_lsit();

		}

		private void button3_Click_1(object sender, EventArgs e)
		{
			MAK_DB_FINAL_lsit();
		}





		// 주어진 폴더와 하위 폴더에서 XML 파일 경로를 찾는 메서드
		public void FindXmlFiles(string folderPath, List<string> filePaths)
		{
			try
			{
				// 현재 폴더의 모든 XML 파일을 찾습니다
				foreach (string file in Directory.GetFiles(folderPath, "*.nfo"))
				{
					filePaths.Add(file);
				}
				//foreach (string file in Directory.GetFiles(folderPath, "*.xml"))
				//{
				//	filePaths.Add(file);
				//}
				// 하위 폴더를 재귀적으로 탐색합니다
				foreach (string subFolder in Directory.GetDirectories(folderPath))
				{
					FindXmlFiles(subFolder, filePaths);
				}
			}
			catch (Exception ex)
			{
				// 폴더 접근 오류 처리
				Console.WriteLine($"폴더 '{folderPath}'을 탐색하는 중 오류 발생: {ex.Message}");
			}
		}



		//1. c#에서 tag list를 읽어서 얼마나 자주사용됬는지 tag_use_lsit.txt file을 만듬
		//MAKE_FILEtag_use_lsit();
		public void MAKE_FILEtag_use_lsit()
		{

			Queue<string> keywordQueue = new Queue<string>();
			// 현재 실행 중인 프로그램의 디렉토리를 기준으로 상대 경로 설정
			//string relativeFolderPath = @"..\..\";
			string relativeFolderPath = @".";
			//
			// XML 파일 경로를 저장할 리스트
			List<string> xmlFilePaths = new List<string>();

			// 폴더와 하위 폴더의 모든 XML 파일 경로를 찾습니다
			FindXmlFiles(relativeFolderPath, xmlFilePaths);


			// 상대 경로를 절대 경로로 변환
			//string folderPath = Path.GetFullPath(xmlFilePaths);

			// 폴더 안의 모든 XML 파일 경로를 배열에 저장
			//string[] xmlFiles = Directory.GetFiles(folderPath, "*.xml");
			//string[] xmlFiles = Directory.GetFiles(folderPath, "*.nfo");


			// 배열에 저장된 XML 파일들의 경로 출력 (확인용)
			//foreach (string file in xmlFilePaths)
			//{
			//	Debug.WriteLine(file);
			//}

			// 배열의 각 XML 파일을 처리하는 예제 (예: 특정 태그 변경)
			foreach (string xmlFile in xmlFilePaths)
			{
				// XML 문서 로드
				XDocument doc = XDocument.Load(xmlFile);
				// 특정 태그 변경 (예: name 태그의 값을 변경)
				foreach (var element_tag in doc.Descendants("tag"))
				{
					// 원하는 새로운 값으로 변경
					//element.Value = "NewName"; 
					//Debug.WriteLine("{0} tag 입니다. .", element_tag);

					// 단어가 키워드 목록에 있는지 확인하고 큐에 추가
					//string s_element_tag = element_tag.ToString();
					string s_element_tag = element_tag.Value.ToString();
					bool exists = keywordQueue.Contains(s_element_tag);//que에있는가?
					if (!exists)
					{
						keywordQueue.Enqueue(s_element_tag);//q item이 추가
					}
					//Debug.WriteLine($"{xmlFile}이(가) 성공적으로 변경되었습니다.");
				}


				//Debug.WriteLine("큐에 저장된 키워드:");
				//foreach (string keyword in keywordQueue)
				//{
				//	Debug.WriteLine(keyword);
				//}



				//end
			}





			string saveas;
			saveas = "tag_use_lsit" + "_s" + ".xml";
			//XDocument doc2 = XDocument.Load(saveas);

			//if (!File.Exists(saveas))				{
			// 빈 XML 문서 생성
			XDocument doc2 = new XDocument(new XElement("root"));

			// 빈 XML 문서를 파일로 저장
			doc2.Save(saveas);

			foreach (string keyword in keywordQueue)
			{
				//Debug.WriteLine(keyword);

				XElement newElement = new XElement("tag", keyword);

				// XML 구조에 따라 원하는 위치에 태그 추가
				// 예를 들어, 루트 요소 아래에 태그를 추가
				doc2.Root.Add(newElement);
				//Debug.WriteLine($" 생성되었습니다: {saveas}");
				doc2.Save(saveas);
			}
			//}
			//else
			//{
			//	// 파일이 존재할 경우, XML 문서를 로드
			//	XDocument doc2 = XDocument.Load(saveas);
			//	foreach (string keyword in keywordQueue)
			//	{
			//		Console.WriteLine($"기존 XML 파일이 로드되었습니다: {saveas}");
			//		XElement newElement = new XElement("tag", keywordQueue);
			//		doc2.Root.Add(newElement);
			//		// 변경된 XML을 파일에 저장
			//		doc2.Save(saveas);
			//		// 추가 작업 수행 가능
			//	}
			//}
 


		}

		public class C_keywordQueue
		{
			public string org_string { get; set; }
			public string change_string { get; set; }

 
			public C_keywordQueue(string org_string2, string change_string2)
			{
				org_string = org_string2;
				change_string = change_string2;
			}
			public override string ToString()
			{
				return $"org_string: {org_string}, change_string: {change_string}";
			}
		}



		//2. 그파일을 통해서 어떻게 변경할지 how_to_chage_list.txt 파일을 만듬
		//GET_DB_FROMFILEtag_use_lsit();
		Queue<C_keywordQueue> g_keywordQueue = new Queue<C_keywordQueue>();
		public void GET_DB_FROMFILEtag_use_lsit()
		{

			 string saveas;   
			saveas = "tag_use_lsit" + "_s" + ".xml";
 
			g_keywordQueue.Clear();
			if (!File.Exists(saveas))	{
				Debug.WriteLine($"{saveas}file을 찾을수 없습니다. .");
				Console.WriteLine($"{saveas}file을 찾을수 없습니다. .");
				return;
			}		
			XDocument doc = XDocument.Load(saveas);
			// 특정 태그 변경 (예: name 태그의 값을 변경) 

			foreach (var element_tag in doc.Descendants("tag"))
			{
				// 원하는 새로운 값으로 변경
				//element.Value = "NewName"; 
				//Debug.WriteLine("{0} tag 입니다. .", element_tag);
 
				// 단어가 키워드 목록에 있는지 확인하고 큐에 추가
				//string s_element_tag = element_tag.ToString();
				string s_element_tag = element_tag.Value.ToString();
				//s_element_tag  = "ABC->ㄱㄴㄷ"
				string[] parts =   s_element_tag.Split('-');

				if (g_keywordQueue.Count==0){
					if (parts.Count() == 2)
					{ 
						g_keywordQueue.Enqueue(new C_keywordQueue(parts[0], parts[1]));
					}
					else{ 
						g_keywordQueue.Enqueue(new C_keywordQueue(parts[0], parts[0]));
					}
				}
				foreach (var for_qc in g_keywordQueue)
				{
					if (for_qc.org_string != parts[0])
					{
						if (parts.Count() == 2)
						{
							g_keywordQueue.Enqueue(new C_keywordQueue(parts[0], parts[1]));
							break;
						}
						else
						{
							g_keywordQueue.Enqueue(new C_keywordQueue(parts[0], parts[0]));
							break;
						}
						
					} 
				}

			}
		}




		// "ABC -> ㄱㄴㄷ"  
		// comparedata : "ABC"
		//changedata : "ㄱㄴㄷ"
		//
		public string  CHANGE_DB_FROMFILEtag_use_lsit(string changed_item )
		{
			string changedata = changed_item;
			foreach (var for_qc in g_keywordQueue)
			{
				if(for_qc.org_string == changed_item){
					changed_item = for_qc.change_string;
					return changed_item;

				}
			}

			Debug.WriteLine($"{changed_item}못찾은게 있습니다. 확인하세요 .");
			Console.WriteLine($"{changed_item}못찾은게 있습니다. 확인하세요 .");
			//bool exists = g_keywordQueue.Contains(changed_item);//que에있는가?
			//if (!exists)
			//{
			//	g_keywordQueue.Enqueue(s_element_tag);//q item이 추가
			//}
			////Debug.WriteLine($"{xmlFile}이(가) 성공적으로 변경되었습니다."); 

			return changed_item;

		}


		//3. how_to_chage_list.txt 을 통해서 폴더안의 .nfo를 일괄  번역하여 저장 
		//MAK_DB_FINAL_lsit();

		public void MAK_DB_FINAL_lsit()
		{ 
			Queue<string> keywordQueue = new Queue<string>(); 
			string relativeFolderPath = @"."; 
			// XML 파일 경로를 저장할 리스트
			List<string> xmlFilePaths = new List<string>(); 
			// 폴더와 하위 폴더의 모든 XML 파일 경로를 찾습니다
			FindXmlFiles(relativeFolderPath, xmlFilePaths); 
			// 배열의 각 XML 파일을 처리하는 예제 (예: 특정 태그 변경)
			foreach (string xmlFile in xmlFilePaths)
			{ 
				//	Debug.WriteLine(file); 
				XDocument doc = XDocument.Load(xmlFile);
				// 특정 태그 변경 (예: name 태그의 값을 변경)
				foreach (var element_tag in doc.Descendants("tag"))
				{
					string string_buffer = CHANGE_DB_FROMFILEtag_use_lsit(element_tag.Value);
					Debug.WriteLine("{0}->{1} from{2}", element_tag.Value, string_buffer, xmlFile);
					Console.WriteLine("{0}->{1}  from{2}", element_tag.Value, string_buffer, xmlFile);
					element_tag.Value = string_buffer;
				}
				doc.Save(xmlFile);
				//end
			}

		}  
	} 

}

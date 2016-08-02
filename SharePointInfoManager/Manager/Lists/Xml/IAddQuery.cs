using System;
namespace SharePointManager.Manager.Lists.Xml {
	interface IAddQuery {
		void AddQuery<TOperator>(string name) where TOperator : QueryOperator, new();
		void AddQuery<TOperator>(string name, DateTime value) where TOperator : QueryOperator, new();
		void AddQuery<TOperator>(string name, int value) where TOperator : QueryOperator, new();
		void AddQuery<TOperator>(string name, string type, string value) where TOperator : QueryOperator, new();
		void AddQuery<TOperator>(string name, string value) where TOperator : QueryOperator, new();
	}
}

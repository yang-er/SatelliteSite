﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace SatelliteSite.Data.Submit
{
    /// <summary>
    /// 表示一份提交的代码
    /// </summary>
    public class Submission
    {
        /// <summary>
        /// 代码的唯一编号
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// 代码提交中的所有文件
        /// </summary>
        public List<File> Files { get; set; }
        
        /// <summary>
        /// 代码的所有Token
        /// </summary>
        public ICollection<Token> Tokens { set; get; }
        
        /// <summary>
        /// 代码使用的语言
        /// </summary>
        public string Language { get; set; }
    }
}
﻿using System;

namespace SatelliteSite.Entities
{
    /// <summary>
    /// 查重报告
    /// </summary>
    public class PlagiarismReport
    {
        /// <summary>
        /// 查重报告的编号
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// A提交的GUID
        /// </summary>
        public int SubmissionA { get; set; }

        /// <summary>
        /// B提交的GUID
        /// </summary>
        public int SubmissionB { get; set; }

        /// <summary>
        /// 总计匹配成功的Token长度
        /// </summary>
        public int TokensMatched { get; set; }

        /// <summary>
        /// 最长连续匹配成功的Token长度
        /// </summary>
        public int BiggestMatch { get; set; }

        /// <summary>
        /// 抄袭百分比
        /// </summary>
        public double Percent { get; set; }

        /// <summary>
        /// A的抄袭百分比
        /// </summary>
        public double PercentA { get; set; }

        /// <summary>
        /// B的抄袭百分比
        /// </summary>
        public double PercentB { get; set; }

        /// <summary>
        /// 此报告是否等待处理
        /// </summary>
        public bool Pending { get; set; }

        /// <summary>
        /// 匹配结果的数组
        /// </summary>
        public byte[] Matches { get; set; }
    }
}
﻿using System;

public class QuizQuestion
{
    public int QuestionId { get; set; }
    public int QuizId { get; set; }
    public string QuestionText { get; set; }
    public string OptionA { get; set; }
    public string OptionB { get; set; }
    public string OptionC { get; set; }
    public string OptionD { get; set; }
    public string CorrectOption { get; set; } // 'A', 'B', 'C' lub 'D'
}
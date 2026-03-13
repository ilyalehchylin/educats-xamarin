using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using EduCATS.Networking.Models.SaveMarks;
using EduCATS.Networking.Models.SaveMarks.LabSchedule;
using EduCATS.Networking.Models.SaveMarks.Practicals;
using EduCATS.Pages.SaveLabsAndPracticeMarks.ViewModels;
using NUnit.Framework;

namespace EduCATS.UnitTests.Pages
{
	[TestFixture]
	public class SaveSingleStudentMarkPageViewModelTests
	{
		[Test]
		public void SetMarksLoadsLabMarkCommentAndVisibilityByLabId()
		{
			var viewModel = createWithoutConstructor();
			viewModel.studentName = "Student A";
			viewModel._subGruop = 2;
			viewModel._takedLabs = new TakedLabs {
				Labs = new List<TakedLab> {
					new TakedLab { LabId = 20, ShortName = "Lab 2", SubGroup = 2 }
				}
			};
			viewModel.fullMarksLabs = new LabsVisitingList {
				Students = new List<LaboratoryWorksModel> {
					new LaboratoryWorksModel {
						FullName = "Student A",
						LabsMarks = new List<LabMarks> {
							new LabMarks {
								LabId = 10,
								Mark = "1",
								Comment = "wrong mark by index",
								ShowForStudent = false,
								Date = "28.02.2026"
							},
							new LabMarks {
								LabId = 20,
								Mark = "7",
								Comment = "Saved comment",
								ShowForStudent = true,
								Date = "01.03.2026"
							}
						}
					}
				}
			};

			viewModel.setMarks("Lab 2");

			Assert.AreEqual(7, viewModel.MarkStudent);
			Assert.AreEqual("Saved comment", viewModel.Comment);
			Assert.IsTrue(viewModel.ShowForStud);
			Assert.AreEqual("01.03.2026", viewModel.SelectedDate);
		}

		[Test]
		public void SetMarksResetsValuesWhenSelectionMissing()
		{
			var viewModel = createWithoutConstructor();
			viewModel.studentName = "Student A";
			viewModel._subGruop = 1;
			viewModel._takedLabs = new TakedLabs {
				Labs = new List<TakedLab> {
					new TakedLab { LabId = 10, ShortName = "Lab 1", SubGroup = 1 }
				}
			};
			viewModel.fullMarksLabs = new LabsVisitingList {
				Students = new List<LaboratoryWorksModel> {
					new LaboratoryWorksModel { FullName = "Student A" }
				}
			};

			viewModel.setMarks("Unknown Lab");

			Assert.AreEqual(0, viewModel.MarkStudent);
			Assert.AreEqual(string.Empty, viewModel.Comment);
			Assert.IsFalse(viewModel.ShowForStud);
			Assert.AreEqual(DateTime.Today.ToString("dd.MM.yyyy"), viewModel.SelectedDate);
		}

		[Test]
		public void ApplyLabMarkToLocalCacheUpdatesExistingLabMark()
		{
			var viewModel = createWithoutConstructor();
			viewModel.studentName = "Student A";
			viewModel._subGruop = 1;
			viewModel.SelectedShortName = "Lab 1";
			viewModel.MarkStudent = 9;
			viewModel.Comment = "Updated lab comment";
			viewModel.ShowForStud = true;
			viewModel.SelectedDate = "01.03.2026";
			viewModel._takedLabs = new TakedLabs {
				Labs = new List<TakedLab> {
					new TakedLab { LabId = 10, ShortName = "Lab 1", SubGroup = 1 }
				}
			};
			viewModel.fullMarksLabs = new LabsVisitingList {
				Students = new List<LaboratoryWorksModel> {
					new LaboratoryWorksModel {
						StudentId = 100,
						FullName = "Student A",
						LabsMarks = new List<LabMarks> {
							new LabMarks {
								LabId = 10,
								Mark = "4",
								Comment = "Old",
								ShowForStudent = false,
								Date = "20.02.2026"
							}
						}
					}
				}
			};

			invokePrivateMethod(viewModel, "applyLabMarkToLocalCache");

			var mark = viewModel.fullMarksLabs.Students[0].LabsMarks[0];
			Assert.AreEqual("9", mark.Mark);
			Assert.AreEqual("Updated lab comment", mark.Comment);
			Assert.IsTrue(mark.ShowForStudent);
			Assert.AreEqual("01.03.2026", mark.Date);
		}

		[Test]
		public void ApplyPracticalMarkToLocalCacheAddsPracticalMarkWhenMissing()
		{
			var viewModel = createWithoutConstructor();
			viewModel.studentName = "Student A";
			viewModel._subGruop = 2;
			viewModel.SelectedShortName = "Pr 2";
			viewModel.MarkStudent = 8;
			viewModel.Comment = "Updated practical comment";
			viewModel.ShowForStud = true;
			viewModel.SelectedDate = "01.03.2026";
			viewModel._takedLabs = new TakedLabs {
				Practicals = new List<PracticialMark> {
					new PracticialMark { PracticalId = 22, ShortName = "Pr 2", SubGroup = 2 }
				}
			};
			viewModel.fullPractice = new LabsVisitingList {
				Students = new List<LaboratoryWorksModel> {
					new LaboratoryWorksModel {
						StudentId = 200,
						FullName = "Student A",
						PracticalsMarks = new List<PracticialMarks>()
					}
				}
			};

			invokePrivateMethod(viewModel, "applyPracticalMarkToLocalCache");

			var marks = viewModel.fullPractice.Students[0].PracticalsMarks;
			Assert.AreEqual(1, marks.Count);
			Assert.AreEqual(22, marks[0].PracticalId);
			Assert.AreEqual("8", marks[0].Mark);
			Assert.AreEqual("Updated practical comment", marks[0].Comment);
			Assert.IsTrue(marks[0].ShowForStudent);
			Assert.AreEqual("01.03.2026", marks[0].Date);
		}

		static SaveSingleStudentMarkPageViewModel createWithoutConstructor()
		{
			return (SaveSingleStudentMarkPageViewModel)FormatterServices.GetUninitializedObject(
				typeof(SaveSingleStudentMarkPageViewModel));
		}

		static void invokePrivateMethod(SaveSingleStudentMarkPageViewModel viewModel, string methodName)
		{
			var method = typeof(SaveSingleStudentMarkPageViewModel).GetMethod(
				methodName,
				BindingFlags.Instance | BindingFlags.NonPublic);
			method.Invoke(viewModel, null);
		}
	}
}

﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;

using ImageViewer.Bases;

using System.Windows;

namespace ImageViewer.Services
{
    public interface IDialogService
    {
        bool CheckActivate(string title);

        /// <summary>
        /// 팝업 컨텐츠 설정
        /// </summary>
        /// <param name="vm">컨텐츠 뷰모델</param>
        /// <param name="title">팝업창 타이틀</param>
        /// <param name="dialogHostType">컨텐츠가 표시될 팝업 다이얼로그 호스트 타입</param>
        void Show(ObservableObject vm, string? title, double width, double height, Type dialogWindowHostType, bool isModal = true);

        /// <summary>
        /// 팝업 다이얼로그 정리
        /// </summary>
        void Clear();
    }

    public class DialogService : IDialogService
    {
        public DialogService()
        {
        }

        public bool CheckActivate(string title)
        {
            var popupWin = Application.Current.Windows.Cast<Window>().FirstOrDefault(p => p.Title == title);
            if (popupWin is not null)
            {
                popupWin.Activate();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Show(ObservableObject vm, string? title, double width, double height, Type dialogWindowHostType, bool isModal = true)
        {
            var popupDialog = Activator.CreateInstance(dialogWindowHostType) as IDialog;

            if (popupDialog is null)
            {
                throw new Exception("팝업 다이얼로그를 생성할 수 없습니다. IDialog 타입인지 체크해 보세요");
            }

            // 팝업창 닫힐때 콜백
            popupDialog.CloseCallback = () =>
            {
                popupDialog.CloseCallback = null;

                if (popupDialog.DataContext is PopupDialogViewModelBase vm)
                {
                    vm.Cleanup();
                }
                popupDialog.DataContext = null;
            };

            if (popupDialog.DataContext is PopupDialogViewModelBase viewModel)
            {
                popupDialog.Width = width;
                popupDialog.Height = height;
                popupDialog.Title = title;
                viewModel.PopupVM = vm;

                if (isModal)
                {
                    popupDialog.ShowDialog();
                }
                else
                {
                    popupDialog.Show();
                }
            }
        }

        public void Clear()
        {
            foreach (var window in Application.Current.Windows)
            {
                if (window is IDialog popupDialog)
                {
                    popupDialog.CloseCallback = null;

                    if (popupDialog.DataContext is PopupDialogViewModelBase vm)
                    {
                        vm.Cleanup();
                    }
                    popupDialog.DataContext = null;
                }
            }
        }
    }
}

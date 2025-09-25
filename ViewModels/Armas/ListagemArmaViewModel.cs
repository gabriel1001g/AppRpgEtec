using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AppRpgEtec.Models;
using AppRpgEtec.Services.Armas;
using AppRpgEtec.Services.Personagens;

namespace AppRpgEtec.ViewModels.Armas
{
    public class ListagemArmaViewModel :BaseViewModel
    {
        private ArmaService aService;
        public ObservableCollection<Arma> Armas { get; set; }
        public ListagemArmaViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            aService = new ArmaService(token);
            Armas = new ObservableCollection<Arma>();

            _ = ObterArmas();

            NovoArmaCommand = new Command(async () => { await ExibirCadastroArmas(); });
            RemoverArmaCommand = new Command<Arma>(async (Arma p) => { await RemoverArma(p); });
        }

        public ICommand NovoArmaCommand { get; }
        public ICommand RemoverArmaCommand { get; set; }

        public async Task ObterArmas()
        {
            try
            {
                Armas = await aService.GetArmasAsync();
                OnPropertyChanged(nameof(Armas)); //Informará a VIEW que houve carregamento
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("OPS", ex.Message + "Detalhes: " + ex.InnerException, "OK");
            }
        }

        public async Task ExibirCadastroArmas()
        {
            try
            {
                await Shell.Current.GoToAsync("cadArmaView");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + "Detalhes" + ex.InnerException, "OK");
            }
        }
        private Arma armaSelecionado;
        public Arma ArmaSelecionado
        {
            get { return armaSelecionado; }
            set
            {
                if (value != null)
                {
                    armaSelecionado = value;
                    Shell.Current.GoToAsync($"CadArmaView?pId={armaSelecionado.Id}");
                }
            }


        }

        public async Task RemoverArma(Arma a)
        {
            try
            {
                if (await Application.Current.MainPage.DisplayAlert("Confirmação", $"Confirma a remoção de {a.Nome}?", "Sim", "Não"))
                {
                    await aService.DeleteArmaAsync(a.Id);
                    await Application.Current.MainPage.DisplayAlert("Mensagem", "Personagem removido com sucesso", "ok");
                    _ = ObterArmas();
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message + "Detalhes: " + ex.InnerException, "Ok");
            }
        }
    }
}

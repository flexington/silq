echo " -- Installing Shell -- "

log_dir=/development/logs/$(date +%Y%m%d)
log_file=${log_dir}/shell.log
mkdir -p $log_dir

echo " -- Installing ZSH -- "
apt-get update &>>/dev/null
apt-get install apt-utils -y &>>$log_file
apt-get install zsh -y &>>$log_file
chsh -s $(which zsh) &>>/dev/null

echo " -- Installing Oh-My-ZSH -- "
sh -c "$(curl -fsSL https://raw.githubusercontent.com/robbyrussell/oh-my-zsh/master/tools/install.sh)" <<<"y" &>>$log_file

echo " -- Installing PowerLevel10k -- "
git clone --depth=1 https://github.com/romkatv/powerlevel10k.git ${ZSH_CUSTOM:-$HOME/.oh-my-zsh/custom}/themes/powerlevel10k &>>$log_file

echo " -- Installing Auto Suggestions -- "
git clone https://github.com/zsh-users/zsh-autosuggestions ${ZSH_CUSTOM:-~/.oh-my-zsh/custom}/plugins/zsh-autosuggestions &>>$log_file

echo " -- Installing Syntax Highlighting -- "
git clone https://github.com/zsh-users/zsh-syntax-highlighting.git ${ZSH_CUSTOM:-~/.oh-my-zsh/custom}/plugins/zsh-syntax-highlighting &>>$log_file

echo " -- Installing Nerd Fonts -- "
apt-get update &>>/dev/null
apt-get install -y wget unzip fontconfig &>>$log_file
wget https://github.com/ryanoasis/nerd-fonts/releases/download/v2.1.0/DroidSansMono.zip &>>$log_file
unzip DroidSansMono.zip -d ~/.fonts &>>$log_file
fc-cache -fv &>>$log_file
rm DroidSansMono.zip

echo " -- Installing bat -- "
apt-get install -y bat &>>$log_file

echo " -- Installing fzf -- "
git clone --depth 1 https://github.com/junegunn/fzf.git ~/.fzf &>>$log_file
~/.fzf/install <<<"yyy" &>>$log_file

rm ~/.zshrc
rm ~/.fzf.zsh
cp ./install/shell/.zshrc ~/.zshrc
cp ./install/shell/.fzf.zsh ~/.fzf.zsh
cp ./install/shell/.p10k.zsh ~/.p10k.zsh

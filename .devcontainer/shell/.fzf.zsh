# Setup fzf
# ---------
if [[ ! "$PATH" == */root/.fzf/bin* ]]; then
  PATH="${PATH:+${PATH}:}/root/.fzf/bin"
fi

# Auto-completion
# ---------------
source "/root/.fzf/shell/completion.zsh"

# Key bindings
# ------------
source "/root/.fzf/shell/key-bindings.zsh"

using System;
using System.Collections.Generic;
using System.Text;

namespace LuYao.LightRpc;

public class CSharpStringBuilder
{
    private readonly StringBuilder _builder = new StringBuilder();

    public override string ToString() => _builder.ToString();
    public void AppendLine(string value)
    {
        _builder.Append(this._tabString);
        _builder.AppendLine(value);
    }
    public void AppendLine() => _builder.AppendLine();
    private string _tabString = string.Empty;
    private int _tabs;
    private void SetTabs()
    {
        if (_tabs > 0)
        {
            _tabString = new string(' ', _tabs);
        }
        else
        {
            _tabString = string.Empty;
        }
    }

    private void AddTab()
    {
        _tabs += 4;
        SetTabs();
    }
    private void RemoveTab()
    {
        _tabs -= 4;
        if (_tabs < 0) _tabs = 0;
        SetTabs();
    }
    public IDisposable Tab()
    {
        this.AddTab();
        return new DisposeAction(() => this.RemoveTab());
    }
}

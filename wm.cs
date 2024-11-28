public void ConfiguraPesquisaComErro()
    {
        _server
            .Given(
                Request.Create()
                    .WithPath("/external/ddpositivo")
                    .UsingPost()
                    .WithBody(new JsonPathMatcher("$.ChaveAcesso", _ddPositivoAccessKey))
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("content-type", "application/json")
                    .WithBody(@"
                        {
                            ""HEADER"": {
                                ""INFORMACOES_RETORNO"": {
                                    ""VERSAO"": """",
                                    ""STATUS_RETORNO"": {
                                        ""CODIGO"": ""0"",
                                        ""DESCRICAO"": ""2-CHAVE DE VALIDACAO INVALIDA.""
                                    },
                                    ""CHAVE_CONSULTA"": """",
                                    ""PRODUTO"": """",
                                    ""CLIENTE"": """",
                                    ""DATA_HORA_CONSULTA"": ""20/08/2024 09:37"",
                                    ""TERMINAL"": """",
                                    ""SOLICITANTE"": """",
                                    ""PDF"": """",
                                    ""ENTIDADE"": """",
                                    ""REQUISICAO"": ""0bbd42b55a5d46e8a6745ac7e4a409a6/20082024093701""
                                }
                            }
                        }"));
    }

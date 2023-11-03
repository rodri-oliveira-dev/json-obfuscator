import org.junit.jupiter.api.Test
import org.mockito.Mockito

class ValidarSolicitacaoUseCaseImplTest {

    @Test
    fun `deve chamar validadores na ordem correta`() {
        // Mock das dependências
        val transferenciaRepository = Mockito.mock(TransferenciaRepository::class.java)
        val transferenciaViewMapper = Mockito.mock(TransferenciaViewMapper::class.java)

        // Mock dos validadores
        val validaProduto = Mockito.spy(ValidaProduto())
        val validaPrograma = Mockito.spy(ValidaPrograma())
        val validaProdutoInformado = Mockito.spy(ValidaProdutoInformado())
        // ...mock outros validadores conforme necessário

        // Instanciando a classe a ser testada
        val validarSolicitacaoUseCase = ValidarSolicitacaoUseCaseImpl(
            transferenciaRepository,
            transferenciaViewMapper
        )

        // Usando reflection para substituir a filaDeValidadores pela nossa mockada
        val field = ValidarSolicitacaoUseCaseImpl::class.java.getDeclaredField("filaDeValidadores")
        field.isAccessible = true
        field.set(validarSolicitacaoUseCase, validaProduto)

        // Encadeamento manual dos validadores
        validaProduto.setNext(validaPrograma)
        validaPrograma.setNext(validaProdutoInformado)
        // ...continue o encadeamento conforme necessário

        // Criar um pedido e configuração de programa mockados
        val pedido = Mockito.mock(Pedido::class.java)
        val programaConfig = Mockito.mock(ProgramaConfig::class.java)

        // Execução do método de validação
        validarSolicitacaoUseCase.validar(pedido, programaConfig)

        // Verificação se cada validador foi chamado
        Mockito.verify(validaProduto).check(programaConfig, pedido)
        Mockito.verify(validaPrograma).check(programaConfig, pedido)
        Mockito.verify(validaProdutoInformado).check(programaConfig, pedido)
        // ...verifique os outros validadores conforme necessário
    }
}